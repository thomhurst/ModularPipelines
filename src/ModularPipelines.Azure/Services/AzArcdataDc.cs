using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcdata")]
public class AzArcdataDc
{
    public AzArcdataDc(
        AzArcdataDcConfig config,
        AzArcdataDcDebug debug,
        AzArcdataDcEndpoint endpoint,
        AzArcdataDcStatus status,
        ICommand internalCommand
    )
    {
        Config = config;
        Debug = debug;
        Endpoint = endpoint;
        Status = status;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzArcdataDcConfig Config { get; }

    public AzArcdataDcDebug Debug { get; }

    public AzArcdataDcEndpoint Endpoint { get; }

    public AzArcdataDcStatus Status { get; }

    public async Task<CommandResult> Create(AzArcdataDcCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzArcdataDcDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Export(AzArcdataDcExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzArcdataDcListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzArcdataDcListOptions(), token);
    }

    public async Task<CommandResult> ListUpgrades(AzArcdataDcListUpgradesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzArcdataDcUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzArcdataDcUpdateOptions(), token);
    }

    public async Task<CommandResult> Upgrade(AzArcdataDcUpgradeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzArcdataDcUpgradeOptions(), token);
    }

    public async Task<CommandResult> Upload(AzArcdataDcUploadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}