using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("ams")]
public class AzAmsContentKeyPolicy
{
    public AzAmsContentKeyPolicy(
        AzAmsContentKeyPolicyOption option,
        ICommand internalCommand
    )
    {
        Option = option;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAmsContentKeyPolicyOption Option { get; }

    public async Task<CommandResult> Create(AzAmsContentKeyPolicyCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAmsContentKeyPolicyDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsContentKeyPolicyDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzAmsContentKeyPolicyListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzAmsContentKeyPolicyShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsContentKeyPolicyShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzAmsContentKeyPolicyUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAmsContentKeyPolicyUpdateOptions(), token);
    }
}