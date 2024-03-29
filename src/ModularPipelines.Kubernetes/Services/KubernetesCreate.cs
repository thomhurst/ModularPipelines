using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Kubernetes.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Kubernetes.Services;

[ExcludeFromCodeCoverage]
public class KubernetesCreate(
    KubernetesCreateSecret secret,
    KubernetesCreateService service,
    ICommand internalCommand
    )
{
    private readonly ICommand _command = internalCommand;

    public KubernetesCreateSecret Secret { get; } = secret;

    public KubernetesCreateService Service { get; } = service;

    public async Task<CommandResult> Clusterrole(KubernetesCreateClusterroleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Clusterrolebinding(KubernetesCreateClusterrolebindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Configmap(KubernetesCreateConfigmapOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesCreateConfigmapOptions(), token);
    }

    public async Task<CommandResult> Cronjob(KubernetesCreateCronjobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Deployment(KubernetesCreateDeploymentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Generic(KubernetesCreateGenericOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesCreateGenericOptions(), token);
    }

    public async Task<CommandResult> Ingress(KubernetesCreateIngressOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Job(KubernetesCreateJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Namespace(KubernetesCreateNamespaceOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesCreateNamespaceOptions(), token);
    }

    public async Task<CommandResult> Poddisruptionbudget(KubernetesCreatePoddisruptionbudgetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Priorityclass(KubernetesCreatePriorityclassOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Quota(KubernetesCreateQuotaOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesCreateQuotaOptions(), token);
    }

    public async Task<CommandResult> Role(KubernetesCreateRoleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Rolebinding(KubernetesCreateRolebindingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Serviceaccount(KubernetesCreateServiceaccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesCreateServiceaccountOptions(), token);
    }
}