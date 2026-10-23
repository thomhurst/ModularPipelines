namespace ModularPipelines.UnitTests.Models;

public class CommandLineTests
{
    [Test]
    public async Task CommandLine_StoresToolAndArguments()
    {
        var commandLine = new CommandLine("git", ["add", "--all"]);

        using (Assert.Multiple())
        {
            await Assert.That(commandLine.Tool).IsEqualTo("git");
            await Assert.That(commandLine.Arguments).IsEquivalentTo(new[] { "add", "--all" });
        }
    }

    [Test]
    public async Task CommandLine_ToString_RedactsArguments()
    {
        var commandLine = new CommandLine("git", ["clone", "https://user:secret@example.com/repo?token=secret"]);

        await Assert.That(commandLine.ToString()).IsEqualTo("git <redacted>");
        await Assert.That(commandLine.Arguments[1]).Contains("secret");
    }

    [Test]
    public async Task CommandLine_EmptyArguments_ToStringShowsToolOnly()
    {
        var commandLine = new CommandLine("git", []);

        await Assert.That(commandLine.ToString()).IsEqualTo("git");
    }

    [Test]
    public async Task CommandLine_ArgumentsAreImmutable()
    {
        var args = new List<string> { "add" };
        var commandLine = new CommandLine("git", args);
        args.Add("--all");

        await Assert.That(commandLine.Arguments).Count().IsEqualTo(1);
    }
}
