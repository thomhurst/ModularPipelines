using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;

namespace ModularPipelines.DotNet;

public interface IDotNet
{
    Task<CommandResult> Restore(DotNetRestoreOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Build(DotNetBuildOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Publish(DotNetPublishOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Pack(DotNetPackOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Clean(DotNetCleanOptions options, CancellationToken cancellationToken = default);

    Task<DotNetTestResult> Test(DotNetTestOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Format(DotNetFormatOptions options, CancellationToken cancellationToken = default);

    Task<CommandResult> Version(DotNetOptions? options = null, CancellationToken cancellationToken = default);

    Task<CommandResult> CustomCommand(DotNetCommandOptions options, CancellationToken cancellationToken = default);
}