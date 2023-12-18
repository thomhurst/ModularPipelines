using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt")]
public class AzDtTwin
{
    public AzDtTwin(
        AzDtTwinComponent component,
        AzDtTwinRelationship relationship,
        AzDtTwinTelemetry telemetry,
        ICommand internalCommand
    )
    {
        Component = component;
        Relationship = relationship;
        Telemetry = telemetry;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDtTwinComponent Component { get; }

    public AzDtTwinRelationship Relationship { get; }

    public AzDtTwinTelemetry Telemetry { get; }

    public async Task<CommandResult> Create(AzDtTwinCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDtTwinDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAll(AzDtTwinDeleteAllOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Query(AzDtTwinQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDtTwinShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzDtTwinUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}