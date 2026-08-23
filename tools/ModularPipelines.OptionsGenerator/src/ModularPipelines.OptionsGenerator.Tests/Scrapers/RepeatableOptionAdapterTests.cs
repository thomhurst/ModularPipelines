using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class RepeatableOptionAdapterTests
{
    private static ICliCommandExecutor Executor { get; } =
        new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance);

    private static IHelpTextCache Cache { get; } =
        new HelpTextCache(NullLogger<HelpTextCache>.Instance);

    [Test]
    public async Task Terraform_Recognizes_Multiple_Times_Prose()
    {
        const string helpText = """
            Usage: terraform apply [options]

            Options:
              -var-file=path  Set variables from a file. This flag can be used multiple times.
            """;
        var command = await new TestTerraformCliScraper().Parse(["terraform", "apply"], helpText);

        await AssertRepeatable(command, "-var-file");
    }

    [Test]
    public async Task Pip_Recognizes_Multiline_Multiple_Times_Prose()
    {
        const string helpText = """
            Usage: pip freeze [options]

            General Options:
              -r, --requirement <file>  Install from the given requirements file.
                                        This option can be used multiple times.
            """;
        var command = await new TestPipCliScraper().Parse(["pip", "freeze"], helpText);

        await AssertRepeatable(command, "--requirement");
    }

    [Test]
    public async Task Packer_Recognizes_Repeatable_Prose()
    {
        const string helpText = """
            Usage: packer build [options]

            Options:
              -var-file=path  Set a variable file; repeatable for additional files.
            """;
        var command = await new TestPackerCliScraper().Parse(["packer", "build"], helpText);

        await AssertRepeatable(command, "--var-file");
    }

    [Test]
    public async Task Snyk_Recognizes_Repeated_Prose()
    {
        const string helpText = """
            Usage: snyk monitor [<OPTIONS>]

            Options
              --project-environment=<ENVIRONMENT>
                  Set the project environment. Can be repeated.
            """;
        var command = await new TestSnykCliScraper().Parse(["snyk", "monitor"], helpText);

        await AssertRepeatable(command, "--project-environment");
    }

    private static async Task AssertRepeatable(
        CliCommandDefinition? command,
        string switchName)
    {
        var option = command!.Options.Single(item => item.SwitchName == switchName);
        using (Assert.Multiple())
        {
            await Assert.That(option.AcceptsMultipleValues).IsTrue();
            await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
        }
    }

    private sealed class TestTerraformCliScraper()
        : TerraformCliScraper(
            RepeatableOptionAdapterTests.Executor,
            RepeatableOptionAdapterTests.Cache,
            NullLogger<TerraformCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                ParseUsageSynopsis(commandPath, helpText),
                CancellationToken.None);
    }

    private sealed class TestPipCliScraper()
        : PipCliScraper(
            RepeatableOptionAdapterTests.Executor,
            RepeatableOptionAdapterTests.Cache,
            NullLogger<PipCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(commandPath, helpText, CancellationToken.None);
    }

    private sealed class TestPackerCliScraper()
        : PackerCliScraper(
            RepeatableOptionAdapterTests.Executor,
            RepeatableOptionAdapterTests.Cache,
            NullLogger<PackerCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(
                commandPath,
                helpText,
                ParseUsageSynopsis(commandPath, helpText),
                CancellationToken.None);
    }

    private sealed class TestSnykCliScraper()
        : SnykCliScraper(
            RepeatableOptionAdapterTests.Executor,
            RepeatableOptionAdapterTests.Cache,
            NullLogger<SnykCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(commandPath, helpText, CancellationToken.None);
    }
}
