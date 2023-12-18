using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "perimeter")]
public class AzNetworkPerimeterLinkReference
{
    public AzNetworkPerimeterLinkReference(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Delete(AzNetworkPerimeterLinkReferenceDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkPerimeterLinkReferenceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkPerimeterLinkReferenceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPerimeterLinkReferenceShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkPerimeterLinkReferenceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPerimeterLinkReferenceWaitOptions(), token);
    }
}