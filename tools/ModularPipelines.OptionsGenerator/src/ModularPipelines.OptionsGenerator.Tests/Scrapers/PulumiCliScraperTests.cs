using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class PulumiCliScraperTests
{
    [Test]
    public async Task Env_Run_Command_Operand_Is_Not_A_Command_Group()
    {
        const string helpText = """
            Run a command within an environment.

            Usage:
              pulumi env run <environment-name> -- <command> [args]

            Run a command
              The command receives the environment variables.
            """;

        var scraper = new TestPulumiCliScraper();
        var command = await scraper.Parse(["pulumi", "env", "run"], helpText);

        await Assert.That(scraper.DeclaresCommandGroup(helpText)).IsFalse();
        await Assert.That(command!.PositionalArguments.Select(argument => argument.PropertyName))
            .IsEquivalentTo(["EnvironmentName", "Command", "Args"]);
        var commandArgument = command.PositionalArguments.Single(argument => argument.PropertyName == "Command");
        using (Assert.Multiple())
        {
            await Assert.That(commandArgument.IsRequired).IsTrue();
            await Assert.That(commandArgument.Phase).IsEqualTo(CommandLinePhase.Passthrough);
            await Assert.That(commandArgument.PositionIndex).IsEqualTo(0);
            await Assert.That(commandArgument.PrependOptionTerminator).IsTrue();
        }

        var args = command.PositionalArguments.Single(argument => argument.PropertyName == "Args");
        await Assert.That(args.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(args.IsVariadic).IsTrue();
        await Assert.That(args.Phase).IsEqualTo(CommandLinePhase.Passthrough);
    }

    [Test]
    public async Task Env_Get_Preserves_Required_Environment_And_Optional_Path()
    {
        const string helpText = """
            Get a value within an environment.

            Usage:
              pulumi env get [<org-name>/][<project-name>/]<environment-name>[@<version>] <path> [flags]

            Flags:
              -h, --help   help for get
            """;

        var command = await new TestPulumiCliScraper().Parse(
            ["pulumi", "env", "get"],
            helpText);

        await Assert.That(command).IsNotNull();
        await Assert.That(command!.PositionalArguments).Count().IsEqualTo(2);
        await Assert.That(command.PositionalArguments[0].PropertyName).IsEqualTo("EnvironmentName");
        await Assert.That(command.PositionalArguments[0].IsRequired).IsTrue();
        await Assert.That(command.PositionalArguments[1].PropertyName).IsEqualTo("Path");
        await Assert.That(command.PositionalArguments[1].IsRequired).IsFalse();
        await Assert.That(command.PositionalArguments[1].CSharpType).IsEqualTo("string?");
    }

    [Test]
    public async Task Env_Clone_Preserves_Both_Required_Environment_Operands()
    {
        const string helpText = """
            Clone an existing environment into a new environment.

            Usage:
              pulumi env clone [<org-name>/]<src-project-name>/<src-environment-name> [<dest-project-name>/]<dest-environment-name> [flags]

            Flags:
              -h, --help   help for clone
            """;

        var command = await new TestPulumiCliScraper().Parse(
            ["pulumi", "env", "clone"],
            helpText);

        await Assert.That(command).IsNotNull();
        await Assert.That(command!.PositionalArguments).Count().IsEqualTo(2);
        await Assert.That(command.PositionalArguments.Select(argument => argument.PropertyName))
            .IsEquivalentTo(["SrcEnvironmentName", "DestEnvironmentName"]);
        await Assert.That(command.PositionalArguments.All(argument => argument.IsRequired)).IsTrue();
    }

    [Test]
    public async Task Static_Provider_Credentials_Are_Secret()
    {
        const string helpText = """
            Add static credentials.

            Usage:
              pulumi env provider azure-login static <environment-name> <tenant-id> <subscription-id> <client-id> <client-secret> [flags]
            """;

        var command = await new TestPulumiCliScraper().Parse(
            ["pulumi", "env", "provider", "azure-login", "static"],
            helpText);

        await Assert.That(command).IsNotNull();
        await Assert.That(command!.PositionalArguments.Single(argument => argument.PropertyName == "ClientSecret").IsSecret)
            .IsTrue();
    }

    [Test]
    public async Task Aws_Static_Provider_Access_Keys_Are_Secret()
    {
        const string helpText = """
            Add static credentials.

            Usage:
              pulumi env provider aws-login static <environment-name> <access-key-id> <secret-access-key> [flags]
            """;

        var command = await new TestPulumiCliScraper().Parse(
            ["pulumi", "env", "provider", "aws-login", "static"],
            helpText);

        await Assert.That(command).IsNotNull();
        await Assert.That(command!.PositionalArguments
                .Single(argument => argument.PropertyName == "AccessKeyId").IsSecret)
            .IsTrue();
        await Assert.That(command.PositionalArguments
                .Single(argument => argument.PropertyName == "SecretAccessKey").IsSecret)
            .IsTrue();
    }

    private sealed class TestPulumiCliScraper : PulumiCliScraper
    {
        public TestPulumiCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<PulumiCliScraper>.Instance)
        {
        }

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }

        public bool DeclaresCommandGroup(string helpText) => HelpDeclaresCommandGroup(helpText);
    }
}
