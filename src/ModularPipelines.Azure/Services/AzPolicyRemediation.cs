using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("policy")]
public class AzPolicyRemediation
{
    public AzPolicyRemediation(
        AzPolicyRemediationDeployment deployment,
        ICommand internalCommand
    )
    {
        Deployment = deployment;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzPolicyRemediationDeployment Deployment { get; }

    public async Task<CommandResult> Cancel(AzPolicyRemediationCancelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzPolicyRemediationCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzPolicyRemediationDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzPolicyRemediationListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPolicyRemediationListOptions(), token);
    }

    public async Task<CommandResult> Show(AzPolicyRemediationShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}