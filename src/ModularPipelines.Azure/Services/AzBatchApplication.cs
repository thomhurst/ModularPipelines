using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch")]
public class AzBatchApplication
{
    public AzBatchApplication(
        AzBatchApplicationPackage package,
        AzBatchApplicationSummary summary,
        ICommand internalCommand
    )
    {
        Package = package;
        Summary = summary;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzBatchApplicationPackage Package { get; }

    public AzBatchApplicationSummary Summary { get; }

    public async Task<CommandResult> Create(AzBatchApplicationCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzBatchApplicationDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzBatchApplicationListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Set(AzBatchApplicationSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzBatchApplicationShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}