using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("confidentialledger")]
public class AzConfidentialledgerManagedccfs
{
    public AzConfidentialledgerManagedccfs(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzConfidentialledgerManagedccfsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzConfidentialledgerManagedccfsDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfidentialledgerManagedccfsDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzConfidentialledgerManagedccfsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfidentialledgerManagedccfsListOptions(), token);
    }

    public async Task<CommandResult> Show(AzConfidentialledgerManagedccfsShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfidentialledgerManagedccfsShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzConfidentialledgerManagedccfsUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfidentialledgerManagedccfsUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzConfidentialledgerManagedccfsWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConfidentialledgerManagedccfsWaitOptions(), token);
    }
}