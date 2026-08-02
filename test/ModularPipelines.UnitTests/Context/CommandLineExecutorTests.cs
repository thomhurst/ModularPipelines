using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.Context;

public class CommandLineExecutorTests
{
    [Test]
    public async Task CommandInputUsesReadableObfuscatedPreparedInput()
    {
        const string secret = "command-line-executor-secret";
        var obfuscator = new ReplacingSecretObfuscator(secret);
        var executor = new CommandLineExecutor(obfuscator);
        var options = new CommandExecutionOptions();

        var result = await executor.ExecuteAsync(
            new CommandLine(
                "pwsh",
                ["-NoProfile", "-Command", $"Write-Output '{secret}'"]),
            options);

        using (Assert.Multiple())
        {
            await Assert.That(result.CommandInput).Contains("Write-Output");
            await Assert.That(result.CommandInput).DoesNotContain(secret);
            await Assert.That(obfuscator.Options).IsSameReferenceAs(options);
        }
    }

    private sealed class ReplacingSecretObfuscator(string secret) : ISecretObfuscator
    {
        public object? Options { get; private set; }

        public string Obfuscate(string? input, object? optionsObject)
        {
            Options = optionsObject;
            return input!.Replace(secret, "**********", StringComparison.Ordinal);
        }
    }
}
