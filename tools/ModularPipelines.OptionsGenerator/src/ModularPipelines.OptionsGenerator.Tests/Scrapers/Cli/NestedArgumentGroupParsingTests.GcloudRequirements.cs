using System.ComponentModel.DataAnnotations;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public partial class NestedArgumentGroupParsingTests
{
    [Test]
    public async Task Gcloud_Conditional_Positional_Operand_Requires_Value_When_Selector_Is_Set()
    {
        const string help = """
            NAME
                gcloud example create - create an example
            SYNOPSIS
                gcloud example create [RESOURCE] [--location=LOCATION]
            POSITIONAL ARGUMENTS
                 Resource resource - resource to configure.
                   RESOURCE
                      This positional argument must be specified if any of the other arguments in this group are specified.
                   --location=LOCATION
                      Select the location.
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], help))!;
        var group = command.RequiredAlternativeGroups.Single();
        await Assert.That(group.IsRequired).IsFalse();
        await Assert.That(group.Members.Single(member => member.PropertyName == "Resource").IsRequired).IsTrue();
        var generated = (await new OptionsClassGenerator().GenerateAsync(new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "output",
            Commands = [command],
        })).Single().Content;
        await VerifyGeneratedValidation(generated, command.ClassName, async type =>
        {
            for (var mask = 0; mask < 4; mask++)
            {
                var instance = Activator.CreateInstance(type)!;
                type.GetProperty("Resource")!.SetValue(instance, (mask & 1) != 0 ? "resource" : " ");
                type.GetProperty("Location")!.SetValue(instance, (mask & 2) != 0 ? "location" : null);
                var errors = ((IValidatableObject) instance).Validate(new(instance));
                await Assert.That(!errors.Any()).IsEqualTo(mask != 2);
            }
        });
    }

    [Test]
    [Arguments("REQUIRED FLAGS", true)]
    [Arguments("FLAGS", false)]
    [Arguments("OPTIONAL FLAGS", false)]
    public async Task Gcloud_Ordinary_Nested_Bundles_Inherit_Section_Requiredness(string section, bool required)
    {
        var help = $$"""
            NAME
                gcloud example create - create an example
            {{section}}
                 Configuration:
                   --config=CONFIG
                      Inline configuration.
                   Authentication:
                     --token=TOKEN
                        Access token.
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], help))!;
        await Assert.That(command.Options).Count().IsEqualTo(2);
        await Assert.That(command.Options.All(option => option.IsRequired == required)).IsTrue();
        await Assert.That(GeneratorUtils.RequiresOptionsParameter(command)).IsEqualTo(required);
    }

    [Test]
    [Arguments("Configuration. This must be specified.", false, true, false)]
    [Arguments("Exactly one of these must be specified:", true, true, true)]
    [Arguments("At most one of these can be specified:", true, false, true)]
    public async Task Gcloud_Positional_Bundles_Resolve_Their_Generated_Members(
        string description, bool includeOption, bool required, bool exclusive)
    {
        var help = $$"""
            NAME
                gcloud example create - create an example
            SYNOPSIS
                gcloud example create [CONFIG] [--profile=PROFILE]
            POSITIONAL ARGUMENTS
                 {{description}}
                   CONFIG
                      Inline configuration.
            """ + (includeOption ? "\n       --profile=PROFILE\n          Saved configuration.\n" : "\n");
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], help))!;
        var group = command.RequiredAlternativeGroups.Single();
        string[] expectedMembers = includeOption ? ["Config", "Profile"] : ["Config"];
        await Assert.That(group.PropertyNames).IsEquivalentTo(expectedMembers);
        var operand = command.PositionalArguments.Single();
        var member = group.Members.Single(member => member.PropertyName == "Config");
        await Assert.That(member.OptionSwitch).IsNull();
        await Assert.That(member.PositionalArgumentPositionIndex).IsEqualTo(operand.PositionIndex);
        await Assert.That(member.PositionalArgumentPhase).IsEqualTo(operand.Phase);
        var generated = (await new OptionsClassGenerator().GenerateAsync(new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "output",
            Commands = [command],
        })).Single().Content;
        await VerifyGeneratedValidation(generated, command.ClassName, async type =>
        {
            for (var mask = 0; mask < (includeOption ? 4 : 2); mask++)
            {
                var instance = Activator.CreateInstance(type)!;
                type.GetProperty("Config")!.SetValue(instance, (mask & 1) != 0 ? "config" : " ");
                if (includeOption)
                {
                    type.GetProperty("Profile")!.SetValue(instance, (mask & 2) != 0 ? "profile" : null);
                }

                var errors = ((IValidatableObject) instance).Validate(new(instance));
                var expected = (!required || mask != 0) && (!exclusive || mask != 3);
                await Assert.That(!errors.Any()).IsEqualTo(expected);
            }
        });
    }

    [Test]
    [Arguments("FLAGS", true)]
    [Arguments("REQUIRED FLAGS", true)]
    [Arguments("OPTIONAL FLAGS", false)]
    public async Task Gcloud_Plain_Mandatory_Bundle_Retains_Presence(string section, bool required)
    {
        var help = $$"""
            NAME
                gcloud example create - create an example
            {{section}}
                 Configuration. This must be specified.
                   --config=CONFIG
                      The configuration to use.
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], help))!;
        await Assert.That(GeneratorUtils.RequiresOptionsParameter(command)).IsEqualTo(required);
        if (required)
        {
            var group = command.RequiredAlternativeGroups.Single();
            await Assert.That(group.IsRequired).IsTrue();
            await Assert.That(group.IsChoice).IsFalse();
            await Assert.That(group.PropertyNames).IsEquivalentTo(["Config"]);
        }
    }

    [Test]
    [Arguments(".")]
    [Arguments(":")]
    [Arguments("")]
    public async Task Gcloud_Mandatory_Bundle_Markers_Use_Consistent_Punctuation(string punctuation)
    {
        var help = $$"""
            NAME
                gcloud example create - create an example
            FLAGS
                 Configuration. This must be specified{{punctuation}}
                   --config=CONFIG
                      Inline configuration.
                   --profile=PROFILE
                      Saved configuration.
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], help))!;
        var group = command.RequiredAlternativeGroups.Single();
        await Assert.That(group.IsRequired).IsTrue();
        await Assert.That(group.IsChoice).IsFalse();
        await Assert.That(group.PropertyNames).IsEquivalentTo(["Config", "Profile"]);
        await Assert.That(GeneratorUtils.RequiresOptionsParameter(command)).IsTrue();
    }

    [Test]
    [Arguments(".")]
    [Arguments(":")]
    [Arguments("")]
    public async Task Gcloud_Mandatory_Bundles_Separate_From_Preceding_Choices(string punctuation)
    {
        var help = $$"""
            NAME
                gcloud example create - create an example
            FLAGS
                 At most one of these can be specified:
                   --token=TOKEN
                      Access token.
                   Configuration bundle. This must be specified{{punctuation}}
                   --config=CONFIG
                      Inline configuration.
                   --profile=PROFILE
                      Saved configuration.
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], help))!;
        var bundle = command.RequiredAlternativeGroups.Single(group => group.IsRequired && !group.IsChoice);
        await Assert.That(bundle.PropertyNames).IsEquivalentTo(["Config", "Profile"]);
        await Assert.That(GeneratorUtils.RequiresOptionsParameter(command)).IsTrue();
    }

    [Test]
    public async Task Gcloud_Mid_Sentence_Alternative_Prose_Preserves_Nested_Requirements()
    {
        const string help = """
            NAME
                gcloud example create - create an example
            FLAGS
                 Compute configuration of the job. Or specify existing resources.
                   --project=PROJECT
                      Project to use.
                   Exactly one of these must be specified:
                     --config=CONFIG
                        Inline configuration.
                     --profile=PROFILE
                        Saved configuration.
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], help))!;
        await Assert.That(command.Options.Select(option => option.PropertyName)).IsEquivalentTo(["Project", "Config", "Profile"]);
        var group = command.RequiredAlternativeGroups.Single();
        await Assert.That(group.IsRequired).IsTrue();
        await Assert.That(group.IsMutuallyExclusive).IsTrue();
        await Assert.That(group.PropertyNames).IsEquivalentTo(["Config", "Profile"]);
    }

    [Test]
    public async Task Gcloud_Plain_Mandatory_Bundle_Rejects_Absent_Input_In_Generated_Validation()
    {
        const string help = """
            NAME
                gcloud example create - create an example
            FLAGS
                 --async
                    Run asynchronously.
                 Configuration. This must be specified.
                   --config=CONFIG
                      Inline configuration.
                   --profile=PROFILE
                      Saved configuration.
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], help))!;
        var generated = (await new OptionsClassGenerator().GenerateAsync(new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "output",
            Commands = [command],
        })).Single().Content;
        await VerifyGeneratedValidation(generated, "GcloudExampleCreateOptions", async type =>
        {
            for (var mask = 0; mask < 8; mask++)
            {
                var instance = Activator.CreateInstance(type)!;
                type.GetProperty("Config")!.SetValue(instance, (mask & 1) != 0 ? "config" : " ");
                type.GetProperty("Profile")!.SetValue(instance, (mask & 2) != 0 ? "profile" : null);
                type.GetProperty("Async")!.SetValue(instance, (mask & 4) != 0);
                var errors = ((IValidatableObject) instance).Validate(new(instance));
                await Assert.That(!errors.Any()).IsEqualTo((mask & 3) != 0);
            }
        });
    }

    [Test]
    [Arguments("At most one of these can be specified:", false, true)]
    [Arguments("At least one of these must be specified:", true, false)]
    public async Task Gcloud_Resource_Bundles_Preserve_Explicit_Nested_Choices(string heading, bool required, bool exclusive)
    {
        var help = $$"""
            NAME
                gcloud example create - create an example
            FLAGS
                 Arguments for authentication:
                   --mode=MODE
                      Authentication mode.
                   {{heading}}
                     --token=TOKEN
                        Access token.
                     --profile=PROFILE
                        Saved profile.
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], help))!;
        var group = command.RequiredAlternativeGroups.Single();
        await Assert.That(group.IsRequired).IsEqualTo(required);
        await Assert.That(group.IsMutuallyExclusive).IsEqualTo(exclusive);
        await Assert.That(group.PropertyNames).IsEquivalentTo(["Token", "Profile"]);
        var generated = (await new OptionsClassGenerator().GenerateAsync(new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "output",
            Commands = [command],
        })).Single().Content;
        await VerifyGeneratedValidation(generated, command.ClassName, async type =>
        {
            for (var mask = 0; mask < 8; mask++)
            {
                var instance = Activator.CreateInstance(type)!;
                type.GetProperty("Token")!.SetValue(instance, (mask & 1) != 0 ? "token" : null);
                type.GetProperty("Profile")!.SetValue(instance, (mask & 2) != 0 ? "profile" : null);
                type.GetProperty("Mode")!.SetValue(instance, (mask & 4) != 0 ? "mode" : null);
                var errors = ((IValidatableObject) instance).Validate(new(instance));
                var expected = (!required || (mask & 3) != 0) && (!exclusive || (mask & 3) != 3);
                await Assert.That(!errors.Any()).IsEqualTo(expected);
            }
        });
    }

    [Test]
    [Arguments("FLAGS", false)]
    [Arguments("REQUIRED FLAGS", true)]
    [Arguments("OPTIONAL FLAGS", false)]
    public async Task Gcloud_Flag_Sections_Preserve_Spaced_Default_Annotations(string section, bool required)
    {
        var help = $$"""
            NAME
                gcloud example create - create an example
            {{section}}
                 --retry-policy=POLICY; default=<PolicyValueValuesEnum.on-demand: 1>
                    The retry policy.
                 --------
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], help))!;
        var option = command.Options.Single();
        await Assert.That(option.SwitchName).IsEqualTo("--retry-policy");
        await Assert.That(option.IsRequired).IsEqualTo(required);
    }

    [Test]
    public async Task Gcloud_Required_Schedule_Group_Preserves_Optional_Defaults_And_Resources()
    {
        // Required-flag excerpt rendered from the public Workbench command reference.
        // https://docs.cloud.google.com/sdk/gcloud/reference/workbench/schedules/create
        var help = await File.ReadAllTextAsync(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Gcloud", "workbench-schedules-create.txt"));
        var command = (await CreateGcloudScraper().Parse(["gcloud", "workbench", "schedules", "create"], help))!;

        await Assert.That(command.RequiredOptions.Select(option => option.SwitchName)).IsEquivalentTo([
            "--region", "--cron-schedule", "--display-name", "--execution-display-name",
            "--gcs-output-uri", "--service-account", "--gcs-notebook-uri",
        ]);
        await Assert.That(command.RequiredAlternativeGroups).Count().IsEqualTo(2);
        foreach (var group in command.RequiredAlternativeGroups)
        {
            await Assert.That(group.IsRequired).IsFalse();
            await Assert.That(group.IsChoice).IsFalse();
            await Assert.That(group.Groups).IsEmpty();
        }

        var subnetwork = command.RequiredAlternativeGroups.Single(group => group.PropertyNames.Contains("Subnetwork"));
        await Assert.That(subnetwork.Members.Select(member => member.OptionSwitch!))
            .IsEquivalentTo(["--subnetwork", "--subnetwork-region"]);
        await Assert.That(subnetwork.Members.Where(member => member.IsRequired).Select(member => member.OptionSwitch!))
            .IsEquivalentTo(["--subnetwork"]);
        var key = command.RequiredAlternativeGroups.Single(group => group.PropertyNames.Contains("KmsKey"));
        await Assert.That(key.Members.Select(member => member.OptionSwitch!))
            .IsEquivalentTo(["--kms-key", "--kms-keyring", "--kms-location", "--kms-project"]);
        await Assert.That(key.Members.Where(member => member.IsRequired).Select(member => member.OptionSwitch!))
            .IsEquivalentTo(["--kms-key"]);
        await Assert.That(command.Options.Single(option => option.SwitchName == "--execution-timeout").CSharpType)
            .IsEqualTo("string?");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--region").Description!)
            .DoesNotContain("Configuration of the schedule. This must be specified.");
    }

    [Test]
    public async Task Gcloud_Privateca_Required_Switches_Accept_Documented_Negation()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Gcloud", "privateca-templates-create.txt"));
        var command = (await CreateGcloudScraper().Parse(["gcloud", "privateca", "templates", "create"], help))!;

        await Assert.That(command.RequiredAlternativeGroups.Where(group => group.IsRequired).SelectMany(group => group.PropertyNames))
            .IsEquivalentTo(["CopySans", "NoCopySans", "CopySubject", "NoCopySubject"]);
        await Assert.That(command.RequiredAlternativeGroups.All(group => group.IsMutuallyExclusive)).IsTrue();
    }

    [Test]
    [Arguments("The max running time, as a duration. See gcloud topic datetimes for formatting.", "string?")]
    [Arguments("Timeout specified as a duration, for example 1h or 30m.", "string?")]
    [Arguments("The timeout in seconds.", "int?")]
    public async Task Gcloud_Duration_Syntax_Takes_Precedence_Over_Numeric_Hints(string description, string expectedType)
    {
        var help = $"""
            NAME
                gcloud example create - create an example
            FLAGS
                 --execution-timeout=EXECUTION_TIMEOUT
                    {description}
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], help))!;
        await Assert.That(command.Options.Single().CSharpType).IsEqualTo(expectedType);
    }

    [Test]
    public async Task Gcloud_Duration_Group_Documentation_Does_Not_Change_Numeric_Members()
    {
        const string help = """
            NAME
                gcloud example create - create an example
            FLAGS
                 Configure the duration of the operation.
                   --retry-count=COUNT
                      Number of retries.
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], help))!;
        await Assert.That(command.Options.Single().CSharpType).IsEqualTo("int?");
    }
}
