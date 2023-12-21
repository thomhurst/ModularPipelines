using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot")]
public class AzIotOps
{
    public AzIotOps(
        AzIotOpsAsset asset,
        AzIotOpsMq mq,
        AzIotOpsSupport support,
        ICommand internalCommand
    )
    {
        Asset = asset;
        Mq = mq;
        Support = support;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzIotOpsAsset Asset { get; }

    public AzIotOpsMq Mq { get; }

    public AzIotOpsSupport Support { get; }

    public async Task<CommandResult> Check(AzIotOpsCheckOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotOpsCheckOptions(), token);
    }

    public async Task<CommandResult> Init(AzIotOpsInitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}