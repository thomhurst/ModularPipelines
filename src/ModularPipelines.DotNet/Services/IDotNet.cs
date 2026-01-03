using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.DotNet.Services;

public interface IDotNet
{
    DotNetTool Tool { get; }

    DotNetWorkload DotNetWorkload { get; }

    DotNetRemove Remove { get; }

    DotNetSdk Sdk { get; }

    DotNetAdd Add { get; }

    DotNetList List { get; }

    DotNetNuget Nuget { get; }

    Task<CommandResult> New(DotNetNewOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Restore(DotNetRestoreOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Build(DotNetBuildOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Publish(DotNetPublishOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Run(DotNetRunOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Test(DotNetTestOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Vstest(DotNetVstestOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Pack(DotNetPackOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Migrate(DotNetMigrateOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Clean(DotNetCleanOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Sln(DotNetSlnOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Store(DotNetStoreOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Watch(DotNetWatchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Format(DotNetFormatOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Workload(DotNetWorkloadOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> Msbuild(DotNetMsbuildOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> BuildServer(DotNetBuildServerOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);

    Task<CommandResult> DevCerts(DotNetDevCertsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default);
}
