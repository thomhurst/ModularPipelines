using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf")]
public class AzSfManagedService
{
    public AzSfManagedService(
        AzSfManagedServiceCorrelationScheme correlationScheme,
        AzSfManagedServiceLoadMetrics loadMetrics,
        ICommand internalCommand
    )
    {
        CorrelationScheme = correlationScheme;
        LoadMetrics = loadMetrics;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSfManagedServiceCorrelationScheme CorrelationScheme { get; }

    public AzSfManagedServiceLoadMetrics LoadMetrics { get; }

    public async Task<CommandResult> Create(AzSfManagedServiceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSfManagedServiceDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSfManagedServiceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSfManagedServiceShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSfManagedServiceUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}