using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class PulumiCliScraperTests
{
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
    }
}
