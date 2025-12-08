using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("sentinel")]
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

    public async Task<CommandResult> Delete(AzSentinelEntityQueryDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSentinelEntityQueryDeleteOptions(), token);
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