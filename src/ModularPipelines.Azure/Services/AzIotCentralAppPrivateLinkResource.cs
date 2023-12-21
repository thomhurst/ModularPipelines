using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "app")]
public class AzIotCentralAppPrivateLinkResource
{
    public AzIotCentralAppPrivateLinkResource(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzIotCentralAppPrivateLinkResourceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotCentralAppPrivateLinkResourceListOptions(), token);
    }

    public async Task<CommandResult> Show(AzIotCentralAppPrivateLinkResourceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotCentralAppPrivateLinkResourceShowOptions(), token);
    }
}