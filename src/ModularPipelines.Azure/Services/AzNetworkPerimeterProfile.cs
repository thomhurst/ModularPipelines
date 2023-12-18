using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "perimeter")]
public class AzNetworkPerimeterProfile
{
    public AzNetworkPerimeterProfile(
        AzNetworkPerimeterProfileAccessRule accessRule,
        ICommand internalCommand
    )
    {
        AccessRule = accessRule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkPerimeterProfileAccessRule AccessRule { get; }

    public async Task<CommandResult> Create(AzNetworkPerimeterProfileCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkPerimeterProfileDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPerimeterProfileDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkPerimeterProfileListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkPerimeterProfileShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPerimeterProfileShowOptions(), token);
    }
}