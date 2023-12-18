using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzConfidentialledger
{
    public AzConfidentialledger(
        AzConfidentialledgerManagedccfs managedccfs,
        ICommand internalCommand
    )
    {
        Managedccfs = managedccfs;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzConfidentialledgerManagedccfs Managedccfs { get; }

    public async Task<CommandResult> Create(AzConfidentialledgerCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzConfidentialledgerDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfidentialledgerDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzConfidentialledgerListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfidentialledgerListOptions(), token);
    }

    public async Task<CommandResult> Show(AzConfidentialledgerShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfidentialledgerShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzConfidentialledgerUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfidentialledgerUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzConfidentialledgerWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfidentialledgerWaitOptions(), token);
    }
}