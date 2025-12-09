using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("automation")]
public class AzAutomationHrwg
{
    public AzAutomationHrwg(
        AzAutomationHrwgHrw hrw,
        ICommand internalCommand
    )
    {
        Hrw = hrw;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAutomationHrwgHrw Hrw { get; }

    public async Task<CommandResult> Create(AzAutomationHrwgCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAutomationHrwgDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomationHrwgDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzAutomationHrwgListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzAutomationHrwgShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomationHrwgShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzAutomationHrwgUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomationHrwgUpdateOptions(), token);
    }
}