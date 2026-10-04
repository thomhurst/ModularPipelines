using System.Collections.Concurrent;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.TypeDetection;

public class AzCliMetadataExecutorTests
{
    [Test]
    public async Task ConcurrentHelpUsesOnePythonDiscoveryAndPreservesArguments()
    {
        var inner = new Executor();
        var executor = new AzCliMetadataExecutor(inner);
        var results = await Task.WhenAll(Enumerable.Range(0, 4)
            .Select(_ => executor.ExecuteAsync("az", "acr login --help", workingDirectory: "work")));

        await Assert.That(inner.Calls.Count(call => call.Arguments == "--version")).IsEqualTo(1);
        await Assert.That(inner.Calls.Where(call => call.Command == "azure-python")
            .All(call => call.Arguments.EndsWith(" acr login --help", StringComparison.Ordinal) && call.Directory == "work")).IsTrue();
        await Assert.That(results.All(result => result.StandardOutput.Contains(AzCliMetadataExecutor.MetadataMarker))).IsTrue();
    }

    [Test]
    public async Task MissingPythonLocationStopsUnverifiedGeneration()
    {
        var executor = new AzCliMetadataExecutor(new Executor { Version = "azure-cli 2.84.0" });
        async Task Act() => _ = await executor.ExecuteAsync("az", "acr login --help");
        await Assert.That(Act)
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task MissingArgumentMetadataStopsUnverifiedGeneration()
    {
        var executor = new AzCliMetadataExecutor(new Executor { Help = "Command\n    az acr login : Log in." });
        async Task Act() => _ = await executor.ExecuteAsync("az", "acr login --help");
        await Assert.That(Act).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task NonHelpCommandsPassThrough()
    {
        var inner = new Executor();
        var executor = new AzCliMetadataExecutor(inner);
        await executor.ExecuteAsync("custom-az", "version");
        await Assert.That(inner.Calls.Single().Command).IsEqualTo("custom-az");
        await Assert.That(inner.Calls.Single().Arguments).IsEqualTo("version");
    }

    private sealed class Executor : ICliCommandExecutor
    {
        public string Version { get; init; } = "azure-cli 2.84.0\nPython location 'azure-python'";

        public string Help { get; init; } = AzCliMetadataExecutor.MetadataMarker + "{}";

        public ConcurrentQueue<(string Command, string Arguments, string? Directory)> Calls { get; } = new();

        public Task<CliCommandResult> ExecuteAsync(string command, string arguments, CancellationToken cancellationToken = default, string? workingDirectory = null)
        {
            Calls.Enqueue((command, arguments, workingDirectory));
            return Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardError = string.Empty,
                StandardOutput = arguments == "--version" ? Version : Help,
            });
        }

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) => Task.FromResult(true);
    }
}
