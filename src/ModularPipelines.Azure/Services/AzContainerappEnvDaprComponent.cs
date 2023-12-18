using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "env")]
public class AzContainerappEnvDaprComponent
{
    public AzContainerappEnvDaprComponent(
        AzContainerappEnvDaprComponentResiliency resiliency,
        ICommand internalCommand
    )
    {
        Resiliency = resiliency;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzContainerappEnvDaprComponentResiliency Resiliency { get; }

    public async Task<CommandResult> Init(AzContainerappEnvDaprComponentInitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzContainerappEnvDaprComponentListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Remove(AzContainerappEnvDaprComponentRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Set(AzContainerappEnvDaprComponentSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzContainerappEnvDaprComponentShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}