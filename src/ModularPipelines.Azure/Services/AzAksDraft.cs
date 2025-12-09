using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks")]
public class AzAksDraft
{
    public AzAksDraft(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzAksDraftCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAksDraftCreateOptions(), token);
    }

    public async Task<CommandResult> GenerateWorkflow(AzAksDraftGenerateWorkflowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAksDraftGenerateWorkflowOptions(), token);
    }

    public async Task<CommandResult> SetupGh(AzAksDraftSetupGhOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAksDraftSetupGhOptions(), token);
    }

    public async Task<CommandResult> Up(AzAksDraftUpOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAksDraftUpOptions(), token);
    }

    public async Task<CommandResult> Update(AzAksDraftUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAksDraftUpdateOptions(), token);
    }
}