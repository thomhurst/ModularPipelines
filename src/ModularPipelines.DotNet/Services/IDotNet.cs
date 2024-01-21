using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;

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

    Task<CommandResult> New(DotNetNewOptions options, CancellationToken token = default);

    Task<CommandResult> Restore(DotNetRestoreOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Build(DotNetBuildOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Publish(DotNetPublishOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Run(DotNetRunOptions? options = default, CancellationToken token = default);

    Task<DotNetTestResult> Test(DotNetTestOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Vstest(DotNetVstestOptions options, CancellationToken token = default);

    Task<CommandResult> Pack(DotNetPackOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Migrate(DotNetMigrateOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Clean(DotNetCleanOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Sln(DotNetSlnOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Store(DotNetStoreOptions options, CancellationToken token = default);

    Task<CommandResult> Watch(DotNetWatchOptions options, CancellationToken token = default);

    Task<CommandResult> Format(DotNetFormatOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Workload(DotNetWorkloadOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Msbuild(DotNetMsbuildOptions options, CancellationToken token = default);

    Task<CommandResult> BuildServer(DotNetBuildServerOptions? options = default, CancellationToken token = default);

    Task<CommandResult> DevCerts(DotNetDevCertsOptions? options = default, CancellationToken token = default);
}