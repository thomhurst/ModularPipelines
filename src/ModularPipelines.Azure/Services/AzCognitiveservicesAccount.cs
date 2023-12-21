using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognitiveservices")]
public class AzCognitiveservicesAccount
{
    public AzCognitiveservicesAccount(
        AzCognitiveservicesAccountCommitmentPlan commitmentPlan,
        AzCognitiveservicesAccountDeployment deployment,
        AzCognitiveservicesAccountIdentity identity,
        AzCognitiveservicesAccountKeys keys,
        AzCognitiveservicesAccountNetworkRule networkRule,
        ICommand internalCommand
    )
    {
        CommitmentPlan = commitmentPlan;
        Deployment = deployment;
        Identity = identity;
        Keys = keys;
        NetworkRule = networkRule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCognitiveservicesAccountCommitmentPlan CommitmentPlan { get; }

    public AzCognitiveservicesAccountDeployment Deployment { get; }

    public AzCognitiveservicesAccountIdentity Identity { get; }

    public AzCognitiveservicesAccountKeys Keys { get; }

    public AzCognitiveservicesAccountNetworkRule NetworkRule { get; }

    public async Task<CommandResult> Create(AzCognitiveservicesAccountCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzCognitiveservicesAccountDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzCognitiveservicesAccountListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCognitiveservicesAccountListOptions(), token);
    }

    public async Task<CommandResult> ListDeleted(AzCognitiveservicesAccountListDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCognitiveservicesAccountListDeletedOptions(), token);
    }

    public async Task<CommandResult> ListKinds(AzCognitiveservicesAccountListKindsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCognitiveservicesAccountListKindsOptions(), token);
    }

    public async Task<CommandResult> ListModels(AzCognitiveservicesAccountListModelsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSkus(AzCognitiveservicesAccountListSkusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCognitiveservicesAccountListSkusOptions(), token);
    }

    public async Task<CommandResult> ListUsage(AzCognitiveservicesAccountListUsageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Purge(AzCognitiveservicesAccountPurgeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Recover(AzCognitiveservicesAccountRecoverOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzCognitiveservicesAccountShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowDeleted(AzCognitiveservicesAccountShowDeletedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzCognitiveservicesAccountUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}