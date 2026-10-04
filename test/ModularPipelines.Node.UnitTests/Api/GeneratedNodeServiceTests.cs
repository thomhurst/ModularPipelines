using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Node.Extensions;
using ModularPipelines.Node.Options;
using ModularPipelines.Node.Services;
using ModularPipelines.Options;

namespace ModularPipelines.Node.UnitTests.Api;

public class GeneratedNodeServiceTests
{
    [Test]
    [Arguments("npm")]
    [Arguments("npm org")]
    [Arguments("npx")]
    public async Task Generated_Services_Forward_Execution_Options_And_Cancellation(string service)
    {
        var command = new RecordingCommandContext();
        var services = new ServiceCollection();
        services.AddSingleton<ICommandContext>(command);
        services.RegisterNodeContext();
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var executionOptions = new CommandExecutionOptions { WorkingDirectory = "working-directory", ThrowOnNonZeroExitCode = false };
        using var cancellation = new CancellationTokenSource();

        Task<CommandResult> Invoke() => service switch
        {
            "npm" => scope.ServiceProvider.GetRequiredService<INpm>().InstallAsync(new NpmInstallOptions(), executionOptions, cancellation.Token),
            "npm org" => scope.ServiceProvider.GetRequiredService<INpm>().Org.LsAsync(new NpmOrgLsOptions("example-org"), executionOptions, cancellation.Token),
            _ => scope.ServiceProvider.GetRequiredService<INpx>().ExecuteAsync(new NpxExecuteOptions(), executionOptions, cancellation.Token),
        };

        await Invoke();
        await Assert.That(ReferenceEquals(command.ExecutionOptions, executionOptions)).IsTrue();
        await Assert.That(command.CancellationToken).IsEqualTo(cancellation.Token);
        await Assert.That(command.Options).IsNotNull();

        await cancellation.CancelAsync();
        var exception = await Assert.ThrowsAsync<OperationCanceledException>(Invoke);
        await Assert.That(exception!.CancellationToken).IsEqualTo(cancellation.Token);
    }

    private sealed class RecordingCommandContext : ICommandContext
    {
        public CommandLineToolOptions? Options { get; private set; }

        public CommandExecutionOptions? ExecutionOptions { get; private set; }

        public CancellationToken CancellationToken { get; private set; }

        public Task<CommandResult> ExecuteCommandLineToolAsync(CommandLineToolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
        {
            Options = options;
            ExecutionOptions = executionOptions;
            CancellationToken = cancellationToken;
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(CommandResult.Ok("captured"));
        }
    }
}
