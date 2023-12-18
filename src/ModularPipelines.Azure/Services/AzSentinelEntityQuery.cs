using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel")]
public class AzSentinelEntityQuery
{
    public AzSentinelEntityQuery(
        AzSentinelEntityQueryTemplate template,
        ICommand internalCommand
    )
    {
        Template = template;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSentinelEntityQueryTemplate Template { get; }

    public async Task<CommandResult> Create(AzSentinelEntityQueryCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSentinelEntityQueryDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSentinelEntityQueryListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSentinelEntityQueryShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSentinelEntityQueryShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSentinelEntityQueryUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSentinelEntityQueryUpdateOptions(), token);
    }
}