using Microsoft.Extensions.Logging.Abstractions;
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
    }

    [Test]
    public async Task Enum_Detection_Deduplicates_Case_Variant_Values()
    {
        var definition = AwsCliScraper.TryDetectEnum(
            "TrafficRoutingConfig",
            "AwsDeployCreateDeploymentConfigOptions",
            "Possible values: TimeBasedCanary TimeBasedLinear AllAtOnce timeBasedCanary timeBasedLinear");
        var values = definition!.Values;

        using (Assert.Multiple())
        {
            await Assert.That(values.Select(value => value.CliValue))
                .IsEquivalentTo(["TimeBasedCanary", "TimeBasedLinear", "AllAtOnce"]);
            await Assert.That(values.Select(value => value.MemberName).Distinct().Count())
                .IsEqualTo(values.Count);
        }
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
    public async Task Enum_Detection_Rejects_Numeric_Constraint_Prose()
    {
        var definition = AwsCliScraper.TryDetectEnum(
            "NumberOfNodes",
            "AwsRedshiftModifyClusterOptions",
            "Valid Values: Integer greater than 0");

        await Assert.That(definition).IsNull();
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
            await Assert.That(option.EnumDefinition).IsNull();
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
        using (Assert.Multiple())
        {
            await Assert.That(options.Single(option => option.SwitchName == "--cli-input-json").CSharpType)
                .IsEqualTo("string?");
            await Assert.That(options.Single(option => option.SwitchName == "--cli-input-json").IsFlag)
                .IsFalse();
            await Assert.That(options.Single(option => option.SwitchName == "--generate-cli-skeleton").CSharpType)
                .IsEqualTo("string?");
            await Assert.That(options.Single(option => option.SwitchName == "--generate-cli-skeleton").IsFlag)
                .IsFalse();
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
                            Possible values: TimeBasedCanary TimeBasedLinear AllAtOnce timeBasedCanary
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

    private static CliCommandResult Result(string output) => new()
    {
        StandardOutput = output,
        StandardError = string.Empty,
        ExitCode = string.IsNullOrEmpty(output) ? 1 : 0,
    };
}
