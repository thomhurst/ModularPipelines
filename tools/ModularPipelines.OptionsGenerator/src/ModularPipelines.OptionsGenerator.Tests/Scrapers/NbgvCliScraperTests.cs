using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class NbgvCliScraperTests
{
    [Test]
    public async Task Root_Help_Discovers_All_Commands()
    {
        const string helpText = """
            Description:
              nbgv v3.10.91

            Usage:
              nbgv [command] [options]

            Commands:
              install                   Prepare a project.
              get-version <commit-ish>  Get version information. [default: HEAD]
              set-version <version>     Set version information.
              tag <versionOrRef>        Create a version tag. [default: HEAD]
              get-commits <version>     Find commits for a version.
              cloud                     Set cloud build variables.
              prepare-release <tag>     Prepare a release.
            """;

        var commands = new TestNbgvCliScraper().Extract(helpText);

        await Assert.That(commands).IsEquivalentTo(
        [
            "install",
            "get-version",
            "set-version",
            "tag",
            "get-commits",
            "cloud",
            "prepare-release",
        ]);
    }

    [Test]
    public async Task GetVersion_Parses_Optional_Commit_And_Explicit_Boolean()
    {
        const string helpText = """
            Description:
              Gets the version information for a project.

            Usage:
              nbgv get-version [<commit-ish>] [options]

            Arguments:
              <commit-ish>  The commit/ref to inspect. [default: HEAD]

            Options:
              -p, --project <project>    The project directory.
              -f, --format <format>      Allowed values are: text, json.
              --public-release           Use --public-release=true or --public-release=false.
              -?, -h, --help             Show help.
            """;

        var command = await new TestNbgvCliScraper().Parse(["nbgv", "get-version"], helpText);
        var commit = command!.PositionalArguments.Single();
        var publicRelease = command.Options.Single(option => option.SwitchName == "--public-release");

        await Assert.That(commit.PropertyName).IsEqualTo("CommitIsh");
        await Assert.That(commit.IsRequired).IsFalse();
        await Assert.That(commit.CSharpType).IsEqualTo("string?");
        await Assert.That(publicRelease.IsFlag).IsFalse();
        await Assert.That(publicRelease.CSharpType).IsEqualTo("bool?");
    }

    [Test]
    public async Task Cloud_Parses_Flags_And_Repeatable_Defines()
    {
        const string helpText = """
            Description:
              Sets cloud build variables.

            Usage:
              nbgv cloud [options]

            Options:
              -s, --ci-system <ci-system>  Force a CI system.
              -a, --all-vars               Define all version variables.
              -c, --common-vars            Define common version variables.
              -d, --define <define>        Additional variables. May be specified multiple times.
              --skip-cloud-build-number    Do not set the cloud build number.
              -?, -h, --help               Show help.
            """;

        var command = await new TestNbgvCliScraper().Parse(["nbgv", "cloud"], helpText);
        var define = command!.Options.Single(option => option.SwitchName == "--define");

        await Assert.That(command.Options.Single(option => option.SwitchName == "--all-vars").IsFlag).IsTrue();
        await Assert.That(define.AcceptsMultipleValues).IsTrue();
        await Assert.That(define.CSharpType).IsEqualTo("IEnumerable<string>?");
    }

    [Test]
    public async Task PrepareRelease_Preserves_CamelCase_Switches()
    {
        const string helpText = """
            Description:
              Prepares a release.

            Usage:
              nbgv prepare-release [<tag>] [options]

            Arguments:
              <tag>  Optional prerelease tag.

            Options:
              --nextVersion <nextVersion>            The next version.
              --versionIncrement <versionIncrement>  The version increment.
              --what-if                              Simulate changes.
              -?, -h, --help                         Show help.
            """;

        var command = await new TestNbgvCliScraper().Parse(["nbgv", "prepare-release"], helpText);

        await Assert.That(command!.PositionalArguments.Single().IsRequired).IsFalse();
        await Assert.That(command.Options.Select(option => option.SwitchName))
            .Contains("--nextVersion")
            .And.Contains("--versionIncrement");
    }

    private sealed class TestNbgvCliScraper : NbgvCliScraper
    {
        public TestNbgvCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<NbgvCliScraper>.Instance)
        {
        }

        public List<string> Extract(string helpText) => [.. ExtractSubcommands(helpText)];

        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText)
        {
            var usage = ParseUsageSynopsis(commandPath, helpText);
            return ParseCommandAsync(commandPath, helpText, usage, CancellationToken.None);
        }
    }
}
