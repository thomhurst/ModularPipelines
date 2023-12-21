using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile-network", "pccp")]
public class AzMobileNetworkPccpVersion
{
    public AzMobileNetworkPccpVersion(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzMobileNetworkPccpVersionListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkPccpVersionListOptions(), token);
    }

    public async Task<CommandResult> Show(AzMobileNetworkPccpVersionShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}