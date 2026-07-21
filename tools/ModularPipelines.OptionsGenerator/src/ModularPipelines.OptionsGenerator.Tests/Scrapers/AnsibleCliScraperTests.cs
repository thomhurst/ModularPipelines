using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class AnsibleCliScraperTests
{
    private const string HelpText = """
        usage: ansible [-h] [-B SECONDS] [-e EXTRA_VARS] pattern

        positional arguments:
          pattern               host pattern

        options:
          --vault-id VAULT_IDS  the vault identity to use. This argument may be
                                specified multiple times.
          --become-password-file BECOME_PASSWORD_FILE, --become-pass-file BECOME_PASSWORD_FILE
                                Become password file
          -B SECONDS, --background SECONDS
                                run asynchronously, failing after X seconds
          -C, --check           don't make any changes
          -e EXTRA_VARS, --extra-vars EXTRA_VARS
                                set additional variables. This argument may be
                                specified multiple times.
          -f FORKS, --forks FORKS
                                specify number of parallel processes
          -v, --verbose         print more debug messages

        Connection Options:
          --private-key PRIVATE_KEY_FILE, --key-file PRIVATE_KEY_FILE
                                use this file to authenticate the connection
          -T TIMEOUT, --timeout TIMEOUT
                                override the connection timeout in seconds
        """;

    [Test]
    public async Task Parses_Argparse_Options_With_Values_Aliases_And_Multiline_Descriptions()
    {
        var command = await new TestAnsibleCliScraper().Parse(HelpText);

        await Assert.That(command).IsNotNull();
        await Assert.That(command!.Options).Count().IsEqualTo(9);

        var background = GetOption(command, "--background");
        await Assert.That(background.ShortForm).IsEqualTo("-B");
        await Assert.That(background.CSharpType).IsEqualTo("int?");
        await Assert.That(background.IsNumeric).IsTrue();

        var check = GetOption(command, "--check");
        await Assert.That(check.ShortForm).IsEqualTo("-C");
        await Assert.That(check.IsFlag).IsTrue();

        var verbose = GetOption(command, "--verbose");
        await Assert.That(verbose.IsFlag).IsTrue();

        var extraVariables = GetOption(command, "--extra-vars");
        await Assert.That(extraVariables.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(extraVariables.AcceptsMultipleValues).IsTrue();
        await Assert.That(extraVariables.Description).Contains("specified multiple times");
    }

    [Test]
    public async Task Marks_Required_Pattern_And_Credential_Options()
    {
        var command = await new TestAnsibleCliScraper().Parse(HelpText);

        await Assert.That(command).IsNotNull();
        await Assert.That(command!.PositionalArguments).Count().IsEqualTo(1);
        await Assert.That(command.PositionalArguments[0].PropertyName).IsEqualTo("Pattern");
        await Assert.That(command.PositionalArguments[0].CSharpType).IsEqualTo("string");
        await Assert.That(command.PositionalArguments[0].IsRequired).IsTrue();
        await Assert.That(GetOption(command, "--become-password-file").IsSecret).IsTrue();
        await Assert.That(GetOption(command, "--private-key").IsSecret).IsTrue();
    }

    private static CliOptionDefinition GetOption(CliCommandDefinition command, string switchName) =>
        command.Options.Single(option => option.SwitchName == switchName);

    private sealed class TestAnsibleCliScraper : AnsibleCliScraper
    {
        public TestAnsibleCliScraper()
            : base(
                new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<AnsibleCliScraper>.Instance)
        {
        }

        public Task<CliCommandDefinition?> Parse(string helpText) =>
            ParseCommandAsync(["ansible"], helpText, CancellationToken.None);
    }
}
