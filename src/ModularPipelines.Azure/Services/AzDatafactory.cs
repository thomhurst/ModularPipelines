using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzDatafactory
{
    public AzDatafactory(
        AzDatafactoryActivityRun activityRun,
        AzDatafactoryDataFlow dataFlow,
        AzDatafactoryDataset dataset,
        AzDatafactoryIntegrationRuntime integrationRuntime,
        AzDatafactoryIntegrationRuntimeNode integrationRuntimeNode,
        AzDatafactoryLinkedService linkedService,
        AzDatafactoryManagedPrivateEndpoint managedPrivateEndpoint,
        AzDatafactoryManagedVirtualNetwork managedVirtualNetwork,
        AzDatafactoryPipeline pipeline,
        AzDatafactoryPipelineRun pipelineRun,
        AzDatafactoryTrigger trigger,
        AzDatafactoryTriggerRun triggerRun,
        ICommand internalCommand
    )
    {
        ActivityRun = activityRun;
        DataFlow = dataFlow;
        Dataset = dataset;
        IntegrationRuntime = integrationRuntime;
        IntegrationRuntimeNode = integrationRuntimeNode;
        LinkedService = linkedService;
        ManagedPrivateEndpoint = managedPrivateEndpoint;
        ManagedVirtualNetwork = managedVirtualNetwork;
        Pipeline = pipeline;
        PipelineRun = pipelineRun;
        Trigger = trigger;
        TriggerRun = triggerRun;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDatafactoryActivityRun ActivityRun { get; }

    public AzDatafactoryDataFlow DataFlow { get; }

    public AzDatafactoryDataset Dataset { get; }

    public AzDatafactoryIntegrationRuntime IntegrationRuntime { get; }

    public AzDatafactoryIntegrationRuntimeNode IntegrationRuntimeNode { get; }

    public AzDatafactoryLinkedService LinkedService { get; }

    public AzDatafactoryManagedPrivateEndpoint ManagedPrivateEndpoint { get; }

    public AzDatafactoryManagedVirtualNetwork ManagedVirtualNetwork { get; }

    public AzDatafactoryPipeline Pipeline { get; }

    public AzDatafactoryPipelineRun PipelineRun { get; }

    public AzDatafactoryTrigger Trigger { get; }

    public AzDatafactoryTriggerRun TriggerRun { get; }

    public async Task<CommandResult> ConfigureFactoryRepo(AzDatafactoryConfigureFactoryRepoOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzDatafactoryCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDatafactoryDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDataPlaneAccess(AzDatafactoryGetDataPlaneAccessOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetGitHubAccessToken(AzDatafactoryGetGitHubAccessTokenOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDatafactoryListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryListOptions(), token);
    }

    public async Task<CommandResult> Show(AzDatafactoryShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDatafactoryUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryUpdateOptions(), token);
    }
}