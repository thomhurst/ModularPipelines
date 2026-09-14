using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;
using System.Text.Json;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class AwsCliScraperTests
{
    [Test]
    public async Task Extracts_Services_From_Aws_2_36_29_Help_Fixture()
    {
        var fixturePath = Path.Combine(
            AppContext.BaseDirectory,
            "Fixtures",
            "AwsCli",
            "aws-2.36.29-root-help.json");
        using var fixture = JsonDocument.Parse(await File.ReadAllTextAsync(fixturePath));
        var helpText = fixture.RootElement.GetProperty("help").GetString()!;

        var scraper = new AwsCliScraper(
            new AwsFixtureExecutor(helpText),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AwsCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        await Assert.That(commands.Select(command => command.FullCommand))
            .IsEquivalentTo(["aws accessanalyzer", "aws cloudformation", "aws ec2"]);
    }

    [Test]
    public async Task Scrape_Normalizes_Ansi_Formatted_Section_Headers()
    {
        var scraper = new AwsCliScraper(
            new AwsHelpExecutor(),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AwsCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        await Assert.That(commands.Select(command => command.FullCommand))
            .IsEquivalentTo(["aws ec2 describe-instances"]);

        var instanceIds = commands.Single().Options.Single(option => option.SwitchName == "--instance-ids");
        using (Assert.Multiple())
        {
            await Assert.That(instanceIds.AcceptsMultipleValues).IsTrue();
            await Assert.That(instanceIds.GroupValues).IsTrue();
        }
    }

    [Test]
    public async Task Enum_Detection_Preserves_Case_Variant_Values()
    {
        var definition = AwsCliScraper.TryDetectEnum(
            "TrafficRoutingConfig",
            "AwsDeployCreateDeploymentConfigOptions",
            "Possible values: TimeBasedCanary TimeBasedLinear AllAtOnce timeBasedCanary timeBasedLinear");
        var values = definition!.Values;

        using (Assert.Multiple())
        {
            await Assert.That(values.Select(value => value.CliValue))
                .IsEquivalentTo(["TimeBasedCanary", "TimeBasedLinear", "AllAtOnce", "timeBasedCanary", "timeBasedLinear"]);
            await Assert.That(values.Select(value => value.MemberName).Distinct().Count())
                .IsEqualTo(values.Count);
        }
    }

    [Test]
    [Arguments(1, false)]
    [Arguments(2, true)]
    [Arguments(16, true)]
    [Arguments(20, true)]
    [Arguments(21, false)]
    public async Task Enum_Detection_Uses_Shared_Member_Limits(int count, bool expectEnum)
    {
        var values = Enumerable.Range(1, count).Select(index => $"value{index}").ToArray();
        var definition = AwsCliScraper.TryDetectEnum("Mode", "AwsExampleOptions", $"Possible values: {string.Join(' ', values)}");

        await Assert.That(definition is not null).IsEqualTo(expectEnum);
        if (expectEnum)
        {
            await Assert.That(definition!.Values.Select(value => value.CliValue)).IsEquivalentTo(values);
        }
    }

    [Test]
    [Arguments("Valid values: cluster, cluster-parameter-group, cluster-security-group, cluster-snapshot, and scheduled-action.", "cluster|cluster-parameter-group|cluster-security-group|cluster-snapshot|scheduled-action")]
    [Arguments("Possible values: net8.0, net9.0 or 1st.", "net8.0|net9.0|1st")]
    [Arguments("Allowed values: foo-bar, foo_bar, or PUBLIC.", "foo-bar|foo_bar|PUBLIC")]
    [Arguments("Possible values: and, or", "and|or")]
    [Arguments("Possible values: AND, OR", "AND|OR")]
    [Arguments("Possible values: alpha, AND beta", "alpha|beta")]
    [Arguments("Possible values: foo-, bar", "foo-|bar")]
    [Arguments("Possible values: o, O", "o|O")]
    [Arguments("Possible values: o Event o RequestResponse o DryRun Constraints: max: 20", "Event|RequestResponse|DryRun")]
    public async Task Enum_Detection_Separates_Choices_From_List_Grammar(string description, string expected)
    {
        var definition = AwsCliScraper.TryDetectEnum("Mode", "AwsExampleOptions", description);
        await Assert.That(definition).IsNotNull();
        await Assert.That(definition!.Values.Select(value => value.CliValue)).IsEquivalentTo(expected.Split('|'));
    }

    [Test]
    [Arguments("AND")]
    [Arguments("OR")]
    [Arguments("And")]
    [Arguments("oR")]
    public async Task Enum_Detection_Rejects_Ambiguous_Conjunctions_In_Any_Case(string conjunction)
    {
        var description = $"Possible values: alpha {conjunction} beta";
        await Assert.That(AwsCliScraper.TryDetectEnum("Mode", "AwsExampleOptions", description)).IsNull();
    }

    [Test]
    [Arguments("Valid values: cluster, cluster-secu-\n rity-group, and scheduled-action.")]
    [Arguments("Valid values: cluster, cluster-security-\n group, and scheduled-action.")]
    [Arguments("Valid values: cluster, cluster-secu- rity-group, and scheduled-action.")]
    [Arguments("Possible values: alpha, beta for testing.")]
    [Arguments("Possible values: alpha, beta/gamma")]
    [Arguments("Possible values: alpha,, beta")]
    [Arguments("Possible values: alpha, beta,")]
    [Arguments("Possible values: alpha beta and gamma")]
    public async Task Ambiguous_Or_Incomplete_Choice_Lists_Use_String_Fallback(string description)
    {
        await Assert.That(AwsCliScraper.TryDetectEnum("Mode", "AwsExampleOptions", description)).IsNull();
    }

    [Test]
    [Arguments("cluster-secu-\n        rity-group")]
    [Arguments("cluster-security-\n        group")]
    public async Task Wrapped_Redshift_Source_Type_Preserves_Free_Form_Input_And_Documentation(string wrappedValue)
    {
        var helpText = $"""
            OPTIONS
                   --source-type (string)
                    Valid values: cluster, {wrappedValue}, cluster-snapshot, and scheduled-action.
            """;
        var scraper = new TestAwsCliScraper();
        var command = (await scraper.Parse(["aws", "redshift", "create-event-subscription"], helpText))!;
        var option = command.Options.Single();
        await Assert.That(option.EnumDefinition).IsNull();
        await Assert.That(option.CSharpType).IsEqualTo("string?");
        await Assert.That(option.Description).Contains(wrappedValue.Replace("\n        ", " "));
        var output = await new OptionsClassGenerator().GenerateAsync(scraper.CreateToolDefinition() with { Commands = [command] });
        await Assert.That(output.Single().Content).Contains("string? SourceType");
        await Assert.That(output.Single().Content).Contains("scheduled-action");
    }

    [Test]
    public async Task Lambda_Invocation_Type_Strips_Aws_Bullet_Markers()
    {
        var definition = AwsCliScraper.TryDetectEnum(
            "InvocationType",
            "AwsLambdaInvokeOptions",
            "Possible values: o Event o RequestResponse o DryRun");

        await Assert.That(definition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["Event", "RequestResponse", "DryRun"]);
    }

    [Test]
    public async Task Enum_Detection_Preserves_Punctuation_And_Leading_Digits()
    {
        var definition = AwsCliScraper.TryDetectEnum("Mode", "AwsExampleOptions",
            "Possible values: foo-bar foo_bar net8.0 net9.0 1st");

        await Assert.That(definition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["foo-bar", "foo_bar", "net8.0", "net9.0", "1st"]);
        await Assert.That(definition.Values.Select(value => value.MemberName).Distinct().Count()).IsEqualTo(5);
    }

    [Test]
    [Arguments("Possible values: active inactive. The default is active.", "active|inactive")]
    [Arguments("Possible values: net8.0, net9.0. The default is net8.0.", "net8.0|net9.0")]
    [Arguments("Valid values: foo-bar foo_bar 1st. Use the first value.", "foo-bar|foo_bar|1st")]
    public async Task Enum_Detection_Stops_Before_Following_Sentence(string description, string expected)
    {
        var definition = AwsCliScraper.TryDetectEnum("Mode", "AwsExampleOptions", description);

        await Assert.That(definition).IsNotNull();
        await Assert.That(definition!.Values.Select(value => value.CliValue)).IsEquivalentTo(expected.Split('|'));
    }

    [Test]
    [Arguments("Possible values: 0, 1, 2")]
    [Arguments("Possible values: 1.5 2.5")]
    public async Task Numeric_Hints_Do_Not_Create_Enums(string description)
    {
        await Assert.That(AwsCliScraper.TryDetectEnum("Value", "AwsExampleOptions", description)).IsNull();
    }

    [Test]
    public async Task Enum_Detection_Rejects_Numeric_Constraint_Prose()
    {
        var definition = AwsCliScraper.TryDetectEnum(
            "NumberOfNodes",
            "AwsRedshiftModifyClusterOptions",
            "Valid Values: Integer greater than 0");

        await Assert.That(definition).IsNull();
    }

    [Test]
    public async Task Enum_Detection_Preserves_Integer_Enum_Value()
    {
        var definition = AwsCliScraper.TryDetectEnum(
            "Unit",
            "AwsConnectUpdateMetricContentOptions",
            "Possible values: o INTEGER o DOUBLE o PERCENT o SECONDS");

        await Assert.That(definition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["INTEGER", "DOUBLE", "PERCENT", "SECONDS"]);
    }

    [Test]
    public async Task Redshift_Number_Of_Nodes_Uses_Numeric_Hint_Before_Constraint_Prose()
    {
        var scraper = new AwsCliScraper(
            new AwsNumericConstraintHelpExecutor(),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AwsCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var option = commands.Single().Options.Single();
        using (Assert.Multiple())
        {
            await Assert.That(option.SwitchName).IsEqualTo("--number-of-nodes");
            await Assert.That(option.CSharpType).IsEqualTo("int?");
            await Assert.That(option.IsNumeric).IsTrue();
            await Assert.That(option.EnumDefinition).IsNull();
        }
    }

    [Test]
    public async Task Structure_Options_Are_Rendered_As_A_Single_Value()
    {
        var scraper = new AwsCliScraper(
            new AwsStructureHelpExecutor(),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AwsCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var option = commands.Single().Options.Single();
        using (Assert.Multiple())
        {
            await Assert.That(option.IsKeyValue).IsFalse();
            await Assert.That(option.CSharpType).IsEqualTo("string?");
            await Assert.That(option.AcceptsMultipleValues).IsFalse();
            await Assert.That(option.EnumDefinition).IsNull();
        }
    }

    [Test]
    public async Task List_Options_Do_Not_Mine_Nested_Enum_Values()
    {
        var scraper = new AwsCliScraper(
            new AwsListHelpExecutor(),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AwsCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var option = commands.Single().Options.Single();
        using (Assert.Multiple())
        {
            await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(option.AcceptsMultipleValues).IsTrue();
            await Assert.That(option.GroupValues).IsTrue();
            await Assert.That(option.EnumDefinition).IsNull();
        }
    }

    [Test]
    public async Task Map_Options_Join_Entries_Into_One_Operand()
    {
        var scraper = new AwsCliScraper(
            new AwsMapHelpExecutor(),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AwsCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var option = commands.Single().Options.Single();
        using (Assert.Multiple())
        {
            await Assert.That(option.CSharpType).IsEqualTo("IReadOnlyList<KeyValue>?");
            await Assert.That(option.IsKeyValue).IsTrue();
            await Assert.That(option.GroupValues).IsFalse();
            await Assert.That(option.CollectionSeparator).IsEqualTo(",");
        }
    }

    [Test]
    public async Task Value_Taking_Global_Command_Options_Are_Not_Flags()
    {
        var scraper = new AwsCliScraper(
            new AwsValueOptionHelpExecutor(),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AwsCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var options = commands.Single().Options;
        var cliInputJson = options.Single(option => option.SwitchName == "--cli-input-json");
        var generateCliSkeleton = options.Single(option => option.SwitchName == "--generate-cli-skeleton");
        using (Assert.Multiple())
        {
            await Assert.That(cliInputJson.CSharpType).IsEqualTo("string?");
            await Assert.That(cliInputJson.IsFlag).IsFalse();
            await Assert.That(cliInputJson.GroupValues).IsFalse();
            await Assert.That(generateCliSkeleton.CSharpType).IsEqualTo("string?");
            await Assert.That(generateCliSkeleton.IsFlag).IsFalse();
            await Assert.That(generateCliSkeleton.GroupValues).IsFalse();
        }
    }

    [Test]
    public async Task Paired_Boolean_Switches_Become_One_Negatable_Option()
    {
        var scraper = new AwsCliScraper(
            new AwsNegatedBooleanHelpExecutor(),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AwsCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var option = commands.Single().Options.Single();
        using (Assert.Multiple())
        {
            await Assert.That(option.SwitchName).IsEqualTo("--associate-public-ip-address");
            await Assert.That(option.NegatedSwitchName).IsEqualTo("--no-associate-public-ip-address");
            await Assert.That(option.PropertyName).IsEqualTo("AssociatePublicIpAddress");
            await Assert.That(option.CSharpType).IsEqualTo("bool?");
            await Assert.That(option.IsFlag).IsTrue();
        }
    }

    [Test]
    [Arguments("--long-enabled |\n       --no-long-enabled", true)]
    [Arguments("--long-enabled\n       | --no-long-enabled", true)]
    [Arguments("--no-long-enabled |\n       --long-enabled", true)]
    [Arguments("[--long-enabled |\n       --no-long-enabled]", false)]
    [Arguments("--long-enabled | --no-long-\n       enabled", true)]
    [Arguments("--no-long-\n       enabled | --long-enabled", true)]
    [Arguments("[--long-enabled | --no-long-\n       enabled]", false)]
    [Arguments("--long-\n       enabled | --no-long-\n       enabled", true)]
    public async Task Wrapped_Boolean_Alternatives_Preserve_Requiredness(string synopsis, bool required)
    {
        var helpText = $"""
            SYNOPSIS
                   aws fixture apply
                   {synopsis}
                   --name <value>

            OPTIONS
                   "--long-enabled" | "--no-long-enabled" (boolean)
                    Enable the feature.

                   --name (string)
                    Resource name.
            """;
        var command = (await new TestAwsCliScraper().Parse(["aws", "fixture", "apply"], helpText))!;
        await Assert.That(command.Options.Single(option => option.SwitchName == "--long-enabled").IsRequired).IsEqualTo(required);
        await Assert.That(command.Options.Single(option => option.SwitchName == "--name").IsRequired).IsTrue();
    }

    [Test]
    [Arguments("[--foo <value> |\n       --bar <value>]")]
    [Arguments("[--foo <value>\n       | --bar <value>]")]
    [Arguments("[[--foo <value>] |\n       --bar <value>]")]
    [Arguments("--foo <value> |\n       --bar <value>")]
    public async Task Wrapped_Choices_Do_Not_Make_Individual_Options_Required(string synopsis)
    {
        var helpText = $"""
            SYNOPSIS
                   aws fixture apply
                   {synopsis}
                   --name <value>

            OPTIONS
                   --foo (string)
                    First input.

                   --bar (string)
                    Second input.

                   --name (string)
                    Resource name.
            """;
        var command = (await new TestAwsCliScraper().Parse(["aws", "fixture", "apply"], helpText))!;
        await Assert.That(command.Options.Single(option => option.SwitchName == "--foo").IsRequired).IsFalse();
        await Assert.That(command.Options.Single(option => option.SwitchName == "--bar").IsRequired).IsFalse();
        await Assert.That(command.Options.Single(option => option.SwitchName == "--name").IsRequired).IsTrue();
    }

    [Test]
    [Arguments("foo-bar")]
    [Arguments("foo2")]
    [Arguments("foo_bar")]
    public async Task Negated_Switch_Prefix_Does_Not_Create_A_Pair(string otherName)
    {
        var helpText = $"""
            OPTIONS
                   --foo (boolean)
                    First feature.

                   "--{otherName}" | "--no-{otherName}" (boolean)
                    Second feature.
            """;
        var command = (await new TestAwsCliScraper().Parse(["aws", "fixture", "apply"], helpText))!;
        await Assert.That(command.Options.Single(option => option.SwitchName == "--foo").NegatedSwitchName).IsNull();
        await Assert.That(command.Options.Single(option => option.SwitchName == $"--{otherName}").NegatedSwitchName)
            .IsEqualTo($"--no-{otherName}");
    }

    [Test]
    public async Task Wrapped_Paired_Boolean_Switches_Remain_Negatable()
    {
        const string helpText = """
            OPTIONS
                   "--include-relational-database-availability-zones" | "--no-include-
              relational-database-availability-zones" (boolean)

                    Whether to include database availability zones.
            """;

        var command = await new TestAwsCliScraper().Parse(
            ["aws", "lightsail", "get-regions"],
            helpText);
        var option = command!.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.SwitchName)
                .IsEqualTo("--include-relational-database-availability-zones");
            await Assert.That(option.NegatedSwitchName)
                .IsEqualTo("--no-include-relational-database-availability-zones");
            await Assert.That(option.CSharpType).IsEqualTo("bool?");
            await Assert.That(option.IsFlag).IsTrue();
        }
    }

    [Test]
    public async Task Explicit_Boolean_Values_And_Scalar_Prose_Preserve_Aws_Shapes()
    {
        var scraper = new AwsCliScraper(
            new AwsShapeValidationHelpExecutor(),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AwsCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var options = commands.Single().Options;
        var enabled = options.Single(option => option.SwitchName == "--enabled");
        var quiet = options.Single(option => option.SwitchName == "--quiet");
        var path = options.Single(option => option.SwitchName == "--entities-path");
        var recipient = options.Single(option => option.SwitchName == "--recipient");
        var tool = scraper.CreateToolDefinition() with { Commands = commands };
        var generatedOptions = await new OptionsClassGenerator().GenerateAsync(tool);
        var generatedContent = generatedOptions.Single(file =>
            file.RelativePath.EndsWith("AwsFixtureApplyOptions.Generated.cs", StringComparison.Ordinal)).Content;
        using (Assert.Multiple())
        {
            await Assert.That(enabled.CSharpType).IsEqualTo("bool?");
            await Assert.That(enabled.IsFlag).IsFalse();
            await Assert.That(enabled.AcceptsMultipleValues).IsFalse();
            await Assert.That(quiet.CSharpType).IsEqualTo("bool?");
            await Assert.That(quiet.IsFlag).IsTrue();
            await Assert.That(quiet.NegatedSwitchName).IsNull();
            await Assert.That(generatedContent).Contains("[CliFlag(\"--quiet\")]");
            await Assert.That(generatedContent).Contains("[CliOption(\"--enabled\")]");
            await Assert.That(path.CSharpType).IsEqualTo("string?");
            await Assert.That(path.AcceptsMultipleValues).IsFalse();
            await Assert.That(recipient.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(recipient.AcceptsMultipleValues).IsTrue();
            await Assert.That(recipient.GroupValues).IsTrue();
        }
    }

    [Test]
    public async Task Enum_Detection_Rejects_Free_Form_Character_Descriptions()
    {
        var definition = AwsCliScraper.TryDetectEnum(
            "MessageGroupId",
            "AwsSqsSendMessageOptions",
            "Valid values: alphanumeric characters and punctuation.");

        await Assert.That(definition).IsNull();
    }

    [Test]
    public async Task Required_Options_Are_Preserved_In_Generated_Apis()
    {
        var scraper = new AwsCliScraper(
            new AwsRequiredOptionsHelpExecutor(),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AwsCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var terminateInstances = commands.Single(command =>
            command.FullCommand == "aws ec2 terminate-instances");
        var createKeyPair = commands.Single(command =>
            command.FullCommand == "aws ec2 create-key-pair");
        var setInstanceProtection = commands.Single(command =>
            command.FullCommand == "aws autoscaling set-instance-protection");
        var sendMessage = commands.Single(command =>
            command.FullCommand == "aws sqs send-message");
        var tool = scraper.CreateToolDefinition() with { Commands = commands };
        var options = await new OptionsClassGenerator().GenerateAsync(tool);
        var services = await new SubDomainClassGenerator().GenerateAsync(tool);
        var terminateOptions = options.Single(file =>
            file.RelativePath.EndsWith("AwsEc2TerminateInstancesOptions.Generated.cs", StringComparison.Ordinal));
        var createOptions = options.Single(file =>
            file.RelativePath.EndsWith("AwsEc2CreateKeyPairOptions.Generated.cs", StringComparison.Ordinal));
        var ec2Interface = services.Single(file =>
            file.RelativePath.EndsWith("IAwsEc2.Generated.cs", StringComparison.Ordinal));
        var autoscalingOptions = options.Single(file =>
            file.RelativePath.EndsWith(
                "AwsAutoscalingSetInstanceProtectionOptions.Generated.cs",
                StringComparison.Ordinal));
        var autoscalingInterface = services.Single(file =>
            file.RelativePath.EndsWith("IAwsAutoscaling.Generated.cs", StringComparison.Ordinal));
        var sendMessageOptions = options.Single(file =>
            file.RelativePath.EndsWith("AwsSqsSendMessageOptions.Generated.cs", StringComparison.Ordinal));
        var sqsInterface = services.Single(file =>
            file.RelativePath.EndsWith("IAwsSqs.Generated.cs", StringComparison.Ordinal));
        var terminateContent = terminateOptions.Content.ReplaceLineEndings("\n");
        var createContent = createOptions.Content.ReplaceLineEndings("\n");
        var autoscalingContent = autoscalingOptions.Content.ReplaceLineEndings("\n");
        var sendMessageContent = sendMessageOptions.Content.ReplaceLineEndings("\n");

        using (Assert.Multiple())
        {
            await Assert.That(terminateInstances.Options.Single(option =>
                option.SwitchName == "--instance-ids").IsRequired).IsTrue();
            await Assert.That(terminateInstances.Options.Single(option =>
                option.SwitchName == "--instance-ids").GroupValues).IsTrue();
            await Assert.That(terminateInstances.Options.Single(option =>
                option.SwitchName == "--dry-run").IsRequired).IsFalse();
            await Assert.That(createKeyPair.Options.Single(option =>
                option.SwitchName == "--key-name").IsRequired).IsTrue();
            await Assert.That(createKeyPair.Options.Single(option =>
                option.SwitchName == "--key-type").IsRequired).IsFalse();
            await Assert.That(setInstanceProtection.Options.Single(option =>
                option.SwitchName == "--protected-from-scale-in").IsRequired).IsTrue();
            await Assert.That(setInstanceProtection.Options.Single(option =>
                option.SwitchName == "--protected-from-scale-in").NegatedSwitchName)
                .IsEqualTo("--no-protected-from-scale-in");
            await Assert.That(sendMessage.Options.Where(option =>
                    option.SwitchName is "--queue-url" or "--message-body")
                .All(option => option.IsRequired)).IsTrue();
            await Assert.That(terminateContent)
                .Contains("public AwsEc2TerminateInstancesOptions(\n        IEnumerable<string> InstanceIds\n    )");
            await Assert.That(terminateContent)
                .Contains("[CliOption(\"--instance-ids\", GroupValues = true)]\n    public IEnumerable<string>? InstanceIds");
            await Assert.That(createContent)
                .Contains("public AwsEc2CreateKeyPairOptions(\n        string KeyName\n    )");
            await Assert.That(createContent)
                .Contains("[CliOption(\"--key-name\")]\n    public string? KeyName");
            await Assert.That(createContent)
                .Contains("public string? KeyName { get; private init; }");
            await Assert.That(terminateContent)
                .Contains("FromCliInputJson(string cliInputJson)");
            await Assert.That(terminateContent)
                .Contains("ForCliSkeleton(string generateCliSkeleton = \"input\")");
            await Assert.That(terminateContent)
                .Contains("generateCliSkeleton is \"input\" or \"yaml-input\"");
            await Assert.That(terminateContent)
                .Contains("Required operation values may only be omitted for input or yaml-input skeletons.");
            await Assert.That(terminateContent)
                .Contains("private AwsEc2TerminateInstancesOptions()");
            await Assert.That(terminateContent)
                .DoesNotContain("public AwsEc2TerminateInstancesOptions()");
            await Assert.That(ec2Interface.Content)
                .Contains("TerminateInstancesAsync(AwsEc2TerminateInstancesOptions options,");
            await Assert.That(ec2Interface.Content)
                .Contains("CreateKeyPairAsync(AwsEc2CreateKeyPairOptions options,");
            await Assert.That(autoscalingContent)
                .Contains("bool ProtectedFromScaleIn\n    )");
            await Assert.That(autoscalingContent)
                .Contains("[CliFlag(\"--protected-from-scale-in\", NegatedName = \"--no-protected-from-scale-in\")]\n    public bool? ProtectedFromScaleIn");
            await Assert.That(autoscalingContent)
                .Contains("public bool? ProtectedFromScaleIn { get; private init; }");
            await Assert.That(autoscalingInterface.Content)
                .Contains("SetInstanceProtectionAsync(AwsAutoscalingSetInstanceProtectionOptions options,");
            await Assert.That(sendMessageContent)
                .Contains("public AwsSqsSendMessageOptions(\n"
                          + "        string QueueUrl,\n"
                          + "        string MessageBody\n    )");
            await Assert.That(sendMessageContent).Contains("ArgumentNullException.ThrowIfNull(QueueUrl)");
            await Assert.That(sendMessageContent).Contains("ArgumentNullException.ThrowIfNull(MessageBody)");
            await Assert.That(sendMessageContent)
                .Contains("[CliOption(\"--queue-url\")]\n    public string QueueUrl { get; private init; }");
            await Assert.That(sendMessageContent)
                .Contains("[CliOption(\"--message-body\")]\n    public string MessageBody { get; private init; }");
            await Assert.That(sqsInterface.Content)
                .Contains("SendMessageAsync(AwsSqsSendMessageOptions options,");
        }
    }

    [Test]
    public async Task High_Level_S3_Commands_Expose_Their_Path_Operands()
    {
        var scraper = new AwsCliScraper(
            new AwsS3HelpExecutor(),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AwsCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var argumentsByCommand = commands.ToDictionary(
            command => command.CommandParts[^1],
            command => command.PositionalArguments);

        using (Assert.Multiple())
        {
            foreach (var command in new[] { "cp", "mv", "sync" })
            {
                await Assert.That(argumentsByCommand[command].Select(argument => argument.PropertyName))
                    .IsEquivalentTo(["Source", "Destination"]);
                await Assert.That(argumentsByCommand[command].All(argument => argument.IsRequired)).IsTrue();
            }

            foreach (var command in new[] { "mb", "presign", "rb", "rm", "website" })
            {
                await Assert.That(argumentsByCommand[command].Single().PropertyName).IsEqualTo("S3Uri");
                await Assert.That(argumentsByCommand[command].Single().IsRequired).IsTrue();
            }

            await Assert.That(argumentsByCommand["ls"].Single().PropertyName).IsEqualTo("S3Uri");
            await Assert.That(argumentsByCommand["ls"].Single().IsRequired).IsFalse();

            var futureArguments = argumentsByCommand["future-command"];
            await Assert.That(futureArguments.Select(argument => argument.PropertyName))
                .IsEquivalentTo(["Source", "Destination"]);
            await Assert.That(futureArguments[0].IsRequired).IsTrue();
            await Assert.That(futureArguments[1].IsRequired).IsFalse();
        }
    }

    [Test]
    public async Task Aws_Command_Families_Preserve_Required_Output_Operands()
    {
        var scraper = new AwsCliScraper(
            new AwsCrossDomainPositionalHelpExecutor(),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AwsCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var expectedCommands = new[]
        {
            "aws lambda invoke",
            "aws s3api get-object",
            "aws bedrock-runtime invoke-model",
        };
        var generated = await new OptionsClassGenerator().GenerateAsync(
            scraper.CreateToolDefinition() with { Commands = commands });

        using (Assert.Multiple())
        {
            await Assert.That(commands.Select(command => command.FullCommand))
                .IsEquivalentTo(expectedCommands);

            foreach (var fullCommand in expectedCommands)
            {
                var command = commands.Single(command => command.FullCommand == fullCommand);
                var outfile = command.PositionalArguments.Single();
                await Assert.That(outfile.PropertyName).IsEqualTo("Outfile");
                await Assert.That(outfile.CSharpType).IsEqualTo("string");
                await Assert.That(outfile.PositionIndex).IsEqualTo(0);
                await Assert.That(outfile.IsRequired).IsTrue();
                await Assert.That(outfile.IsVariadic).IsFalse();
                await Assert.That(outfile.PrependOptionTerminator).IsFalse();
                // AWS CLI 2.36.44 rejects --cli-input-json on these streaming-output commands.
                var content = generated.Single(file => file.RelativePath.EndsWith(
                    $"{command.ClassName}.Generated.cs", StringComparison.Ordinal)).Content;
                await Assert.That(content).DoesNotContain("FromCliInputJson");
                await Assert.That(content).DoesNotContain("ForCliSkeleton");
            }
        }
    }

    [Test]
    public async Task Aws_Synopsis_Operands_Preserve_Repeatability_And_Option_Terminators()
    {
        const string helpText = """
            SYNOPSIS
                   export-objects
                   --filter <value>
                   --
                   <outfile>...

            OPTIONS
                   --filter (string)
                    Object filter.
            """;

        var command = await new TestAwsCliScraper().Parse(
            ["aws", "fixture", "export-objects"],
            helpText);
        var outfile = command!.PositionalArguments.Single();

        using (Assert.Multiple())
        {
            await Assert.That(outfile.PropertyName).IsEqualTo("Outfile");
            await Assert.That(outfile.CSharpType).IsEqualTo("IEnumerable<string>");
            await Assert.That(outfile.PositionIndex).IsEqualTo(0);
            await Assert.That(outfile.IsRequired).IsTrue();
            await Assert.That(outfile.IsVariadic).IsTrue();
            await Assert.That(outfile.PrependOptionTerminator).IsTrue();
        }
    }

    [Test]
    public async Task Nested_Command_Groups_Do_Not_Expose_Command_Placeholders()
    {
        var scraper = new AwsCliScraper(
            new AwsNestedCommandGroupHelpExecutor(),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AwsCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var group = commands.Single(command => command.FullCommand == "aws fixture group");

        await Assert.That(group.PositionalArguments).IsEmpty();
    }

    private sealed class AwsNestedCommandGroupHelpExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            var output = arguments switch
            {
                "help" => "AVAILABLE SERVICES\n       o fixture",
                "fixture help" => "AVAILABLE COMMANDS\n       o group",
                "fixture group help" => """
                    SYNOPSIS
                           group
                           <command>
                           [--configuration <value>]

                    OPTIONS
                           --configuration (string)
                            Group configuration.

                    AVAILABLE COMMANDS
                           o child
                    """,
                "fixture group child help" => """
                    OPTIONS
                           --name (string)
                            Child name.
                    """,
                _ => string.Empty,
            };

            return Task.FromResult(Result(output));
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private sealed class AwsCrossDomainPositionalHelpExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            var output = arguments switch
            {
                "help" => "AVAILABLE SERVICES\n       o bedrock-runtime\n       o lambda\n       o s3api",
                "bedrock-runtime help" => "AVAILABLE COMMANDS\n       o invoke-model",
                "lambda help" => "AVAILABLE COMMANDS\n       o invoke",
                "s3api help" => "AVAILABLE COMMANDS\n       o get-object",
                "bedrock-runtime invoke-model help" => CommandHelp(
                    "invoke-model",
                    "--model-id"),
                "lambda invoke help" => CommandHelp(
                    "invoke",
                    "--function-name"),
                "s3api get-object help" => CommandHelp(
                    "get-object",
                    "--bucket",
                    "--key"),
                _ => string.Empty,
            };

            return Task.FromResult(Result(output));
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);

        private static string CommandHelp(
            string command,
            params string[] requiredOptions)
        {
            var synopsisOptions = string.Join(
                '\n',
                requiredOptions.Select(option => $"       {option} <value>"));
            var documentedOptions = string.Join(
                "\n\n",
                requiredOptions.Select(option =>
                    $"       {option} (string) [required]\n        Required command input."));

            return $$"""
            SYNOPSIS
                   {{command}}
            {{synopsisOptions}}
                   <outfile>
                   [--debug]

            OPTIONS
            {{documentedOptions}}

                   --debug (boolean)
                    Enable debug output.
            """;
        }
    }

    private sealed class AwsHelpExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            var output = arguments switch
            {
                "help" => "\u001b[1mAVAILABLE SERVICES\u001b[0m\n       o ec2",
                "ec2 help" => "\u001b[1mAVAILABLE COMMANDS\u001b[0m\n       o describe-instances",
                "ec2 describe-instances help" => """
                    DESCRIPTION
                           Describes EC2 instances.

                    OPTIONS
                           --instance-ids (list)
                           Instance identifiers.
                    """,
                _ => string.Empty,
            };

            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = output,
                StandardError = string.Empty,
                ExitCode = string.IsNullOrEmpty(output) ? 1 : 0,
            });
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private sealed class AwsFixtureExecutor(string rootHelp) : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                StandardOutput = arguments == "help"
                    ? rootHelp
                    : "OPTIONS\n       --enabled (boolean)\n\n       Enable the command.\n",
                StandardError = string.Empty,
                ExitCode = 0,
            });

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private sealed class AwsStructureHelpExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            var output = arguments switch
            {
                "help" => "AVAILABLE SERVICES\n       o deploy",
                "deploy help" => "AVAILABLE COMMANDS\n       o create-deployment-config",
                "deploy create-deployment-config help" => """
                    OPTIONS
                           --traffic-routing-config (structure)
                            Contains an array of nested values. Possible values: TimeBasedCanary TimeBasedLinear AllAtOnce timeBasedCanary
                    """,
                _ => string.Empty,
            };

            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = output,
                StandardError = string.Empty,
                ExitCode = string.IsNullOrEmpty(output) ? 1 : 0,
            });
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private sealed class AwsNumericConstraintHelpExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            var output = arguments switch
            {
                "help" => "AVAILABLE SERVICES\n       o redshift",
                "redshift help" => "AVAILABLE COMMANDS\n       o modify-cluster",
                "redshift modify-cluster help" => """
                    OPTIONS
                           --number-of-nodes (integer)
                            Valid Values: Integer greater than 0
                    """,
                _ => string.Empty,
            };

            return Task.FromResult(Result(output));
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private sealed class AwsListHelpExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            var output = arguments switch
            {
                "help" => "AVAILABLE SERVICES\n       o ec2",
                "ec2 help" => "AVAILABLE COMMANDS\n       o create-ipam-prefix-list-resolver",
                "ec2 create-ipam-prefix-list-resolver help" => """
                    OPTIONS
                           --rules (list)
                            CIDR selection rules. Possible values: static-cidr ipam-resource-cidr
                            ipam-pool-cidr StaticCidr is the fixed CIDR value.
                    """,
                _ => string.Empty,
            };

            return Task.FromResult(new CliCommandResult
            {
                StandardOutput = output,
                StandardError = string.Empty,
                ExitCode = string.IsNullOrEmpty(output) ? 1 : 0,
            });
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private sealed class AwsValueOptionHelpExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            var output = arguments switch
            {
                "help" => "AVAILABLE SERVICES\n       o ec2",
                "ec2 help" => "AVAILABLE COMMANDS\n       o run-instances",
                "ec2 run-instances help" => """
                    OPTIONS
                           --cli-input-json
                           JSON request document.

                           --generate-cli-skeleton
                           Skeleton output mode.
                    """,
                _ => string.Empty,
            };

            return Task.FromResult(Result(output));
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private sealed class AwsMapHelpExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            var output = arguments switch
            {
                "help" => "AVAILABLE SERVICES\n       o amplify",
                "amplify help" => "AVAILABLE COMMANDS\n       o create-app",
                "amplify create-app help" => """
                    OPTIONS
                           --environment-variables (map)
                            Shorthand Syntax: KeyName1=string,KeyName2=string
                    """,
                _ => string.Empty,
            };

            return Task.FromResult(Result(output));
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private sealed class AwsS3HelpExecutor : ICliCommandExecutor
    {
        private static readonly string[] Commands =
            ["cp", "future-command", "ls", "mb", "mv", "presign", "rb", "rm", "sync", "website"];

        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            var output = arguments switch
            {
                "help" => "AVAILABLE SERVICES\n       o s3",
                "s3 help" => $"AVAILABLE COMMANDS\n{string.Join('\n', Commands.Select(name => $"       o {name}"))}",
                "s3 future-command help" => """
                    SYNOPSIS
                           aws s3 future-command <source> [destination]

                    OPTIONS
                           --quiet (boolean)
                    """,
                _ when arguments.StartsWith("s3 ", StringComparison.Ordinal) => "OPTIONS\n       --quiet (boolean)\n",
                _ => string.Empty,
            };

            return Task.FromResult(Result(output));
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private sealed class AwsNegatedBooleanHelpExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            var output = arguments switch
            {
                "help" => "AVAILABLE SERVICES\n       o ec2",
                "ec2 help" => "AVAILABLE COMMANDS\n       o run-instances",
                "ec2 run-instances help" => """
                    OPTIONS
                           "--associate-public-ip-address" | "--no-associate-public-ip-address"
                            (boolean) [EC2-VPC] If specified a public IP address will be assigned.
                    """,
                _ => string.Empty,
            };

            return Task.FromResult(Result(output));
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private sealed class AwsShapeValidationHelpExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            var output = arguments switch
            {
                "help" => "AVAILABLE SERVICES\n       o fixture",
                "fixture help" => "AVAILABLE COMMANDS\n       o apply",
                "fixture apply help" => """
                    OPTIONS
                           --enabled (boolean)
                            Explicit Boolean value. Possible values: true false

                           --quiet (boolean)
                            Suppress command output.

                           --entities-path (string)
                            A path that contains multiple levels.

                           --recipient (string) May be specified multiple times.
                    """,
                _ => string.Empty,
            };

            return Task.FromResult(Result(output));
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    [Test]
    public async Task Required_Map_Options_Generate_Collection_Guards()
    {
        const string helpText = """
            SYNOPSIS
                   apply
                   --attributes <value>

            OPTIONS
                   --attributes (map) [required]
                    Attribute names and values.
            """;
        var scraper = new TestAwsCliScraper();
        var command = (await scraper.Parse(["fixture", "apply"], helpText))!;
        var option = command.Options.Single();
        await Assert.That(option.CSharpType).IsEqualTo("IReadOnlyList<KeyValue>?");
        await Assert.That(option.IsKeyValue).IsTrue();
        await Assert.That(option.IsCollection).IsNull();

        var generated = await new OptionsClassGenerator().GenerateAsync(
            scraper.CreateToolDefinition() with { Commands = [command] });
        var content = generated.Single().Content;
        await Assert.That(content).Contains("ArgumentNullException.ThrowIfNull(Attributes)");
        await Assert.That(content).Contains("Enumerable.ToArray(global::System.Linq.Enumerable.Cast<KeyValue>(Attributes))");
        await Assert.That(content).Contains("static value => value is not null");
        await Assert.That(content).Contains("Required collection must contain at least one value.");
    }

    private sealed class TestAwsCliScraper()
        : AwsCliScraper(
            new AwsFixtureExecutor(string.Empty),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AwsCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }

    private sealed class AwsRequiredOptionsHelpExecutor : ICliCommandExecutor
    {
        public Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null)
        {
            var output = arguments switch
            {
                "help" => "AVAILABLE SERVICES\n       o autoscaling\n       o ec2\n       o sqs",
                "autoscaling help" => "AVAILABLE COMMANDS\n       o set-instance-protection",
                "autoscaling set-instance-protection help" => """
                    DESCRIPTION
                           Updates the instance protection settings of the specified instances. This operation cannot be called on instances in a warm pool. For more information, see Use instance scale-in protection in the Ama- zon EC2 Auto Scaling User Guide . If you exceed your maximum limit of instance IDs, which is 50 per Auto Scaling group, the call fails. See also: AWS API Documentation

                    SYNOPSIS
                           aws autoscaling set-instance-protection
                           --instance-ids <value>
                           --auto-scaling-group-name <value>
                           --protected-from-scale-in | --no-protected-from-scale-in
                           [--cli-input-json <value>]
                           [--generate-cli-skeleton <value>]

                    OPTIONS
                           --instance-ids (list) [required]
                            Instance identifiers.

                           --auto-scaling-group-name (string) [required]
                            The Auto Scaling group name.

                           "--protected-from-scale-in" | "--no-protected-from-scale-in" (boolean)
                            Whether instances are protected from scale in. Possible values: true false

                           --cli-input-json (string)
                            JSON input.

                           --generate-cli-skeleton (string)
                            Prints a skeleton.
                    """,
                "ec2 help" => "AVAILABLE COMMANDS\n       o create-key-pair\n       o terminate-instances",
                "ec2 terminate-instances help" => """
                    SYNOPSIS
                           aws ec2 terminate-instances
                           --instance-ids <value>
                           [--dry-run | --no-dry-run]
                           [--cli-input-json <value>]
                           [--generate-cli-skeleton <value>]

                    OPTIONS
                           --instance-ids (list)
                            Instance identifiers.

                           --dry-run (boolean)
                            Checks whether you have the required permissions.

                           --cli-input-json (string)
                            JSON input.

                           --generate-cli-skeleton (string)
                            Prints a skeleton.
                    """,
                "ec2 create-key-pair help" => """
                    SYNOPSIS
                           aws ec2 create-key-pair
                           [--key-name <value>]
                           [--key-type <value>]
                           [--cli-input-json <value>]
                           [--generate-cli-skeleton <value>]

                    OPTIONS
                           --key-name (string) [required]
                            A unique name for the key pair.

                           --key-type (string)
                            The type of key pair.

                           --cli-input-json (string)
                            JSON input.

                           --generate-cli-skeleton (string)
                            Prints a skeleton.
                    """,
                "sqs help" => "AVAILABLE COMMANDS\n       o send-message",
                "sqs send-message help" => """
                    SYNOPSIS
                           aws sqs send-message
                           --queue-url <value>
                           --message-body <value>

                    OPTIONS
                           --queue-url (string)
                            The queue URL.

                           --message-body (string)
                            The message body.
                    """,
                _ => string.Empty,
            };

            return Task.FromResult(Result(output));
        }

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    private static CliCommandResult Result(string output) => new()
    {
        StandardOutput = output,
        StandardError = string.Empty,
        ExitCode = string.IsNullOrEmpty(output) ? 1 : 0,
    };
}
