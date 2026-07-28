using ModularPipelines.Exceptions;
using ModularPipelines.Models;

namespace ModularPipelines.UnitTests.Exceptions;

public class CommandExceptionTests
{
    [Test]
    public async Task Constructor_ExposesResultWithoutEmbeddingOutputInMessage()
    {
        var result = new CommandResult(
            commandInput: "tool --token secret",
            workingDirectory: "working-directory",
            standardOutput: "sensitive stdout",
            standardError: "sensitive stderr",
            environmentVariables: new Dictionary<string, string?>(),
            startTime: DateTimeOffset.UtcNow,
            endTime: DateTimeOffset.UtcNow,
            duration: TimeSpan.FromSeconds(1),
            exitCode: 42);
        var innerException = new InvalidOperationException("failure");

        var exception = new CommandException(result, innerException);

        using (Assert.Multiple())
        {
            await Assert.That(exception.Result).IsSameReferenceAs(result);
            await Assert.That(exception.InnerException).IsSameReferenceAs(innerException);
            await Assert.That(exception.Message).IsEqualTo("Command failed with exit code 42.");
            await Assert.That(exception.Message).DoesNotContain(result.CommandInput);
            await Assert.That(exception.Message).DoesNotContain(result.StandardOutput);
            await Assert.That(exception.Message).DoesNotContain(result.StandardError);
        }
    }
}
