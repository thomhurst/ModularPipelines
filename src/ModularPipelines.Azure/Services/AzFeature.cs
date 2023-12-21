using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzFeature
{
    public AzFeature(
        AzFeatureRegistration registration,
        ICommand internalCommand
    )
    {
        Registration = registration;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzFeatureRegistration Registration { get; }

    public async Task<CommandResult> List(AzFeatureListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFeatureListOptions(), token);
    }

    public async Task<CommandResult> Register(AzFeatureRegisterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzFeatureShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Unregister(AzFeatureUnregisterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}