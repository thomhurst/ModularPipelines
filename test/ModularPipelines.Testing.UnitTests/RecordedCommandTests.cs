using ModularPipelines.Options;

namespace ModularPipelines.Testing.UnitTests;

public class RecordedCommandTests
{
    [Test]
    public async Task ToString_UsesMaskedInvocationAndRedactsRawResult()
    {
        const string secret = "raw-command-secret";
        var commandLine = new CommandLine("tool", [$"https://user:{secret}@example.com?token={secret}"]);
        var invocation = new CommandInvocation(commandLine, null, new CommandExecutionOptions(),
            "tool https://user:***@example.com?token=***", "/work", new Dictionary<string, string?>());
        var result = CommandResult.Ok(secret) with { CommandInput = secret };
        var recorded = new RecordedCommand(invocation, result);

        var printed = recorded.ToString();

        await Assert.That(printed).DoesNotContain(secret);
        await Assert.That(printed).Contains(invocation.ToString());
        await Assert.That(printed).Contains("Result = <redacted>");
        await Assert.That(recorded.CommandLine).IsSameReferenceAs(commandLine);
        await Assert.That(recorded.Result).IsSameReferenceAs(result);
    }
}
