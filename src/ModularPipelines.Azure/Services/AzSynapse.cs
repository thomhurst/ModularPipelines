using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzSynapse
{
    public AzSynapse(
        AzSynapseActivityRun activityRun,
        AzSynapseAdOnlyAuth adOnlyAuth,
        AzSynapseDataFlow dataFlow,
        AzSynapseDataset dataset,
        AzSynapseIntegrationRuntime integrationRuntime,
        AzSynapseIntegrationRuntimeNode integrationRuntimeNode,
        AzSynapseKqlScript kqlScript,
        AzSynapseKusto kusto,
        AzSynapseKustoOperation kustoOperation,
        AzSynapseLinkConnection linkConnection,
        AzSynapseLinkedService linkedService,
        AzSynapseManagedPrivateEndpoints managedPrivateEndpoints,
        AzSynapseNotebook notebook,
        AzSynapsePipeline pipeline,
        AzSynapsePipelineRun pipelineRun,
        AzSynapseRole role,
        AzSynapseSpark spark,
        AzSynapseSparkJobDefinition sparkJobDefinition,
        AzSynapseSql sql,
        AzSynapseSqlScript sqlScript,
        AzSynapseTrigger trigger,
        AzSynapseTriggerRun triggerRun,
        AzSynapseWorkspace workspace,
        AzSynapseWorkspacePackage workspacePackage
    )
    {
        ActivityRun = activityRun;
        AdOnlyAuth = adOnlyAuth;
        DataFlow = dataFlow;
        Dataset = dataset;
        IntegrationRuntime = integrationRuntime;
        IntegrationRuntimeNode = integrationRuntimeNode;
        KqlScript = kqlScript;
        Kusto = kusto;
        KustoOperation = kustoOperation;
        LinkConnection = linkConnection;
        LinkedService = linkedService;
        ManagedPrivateEndpoints = managedPrivateEndpoints;
        Notebook = notebook;
        Pipeline = pipeline;
        PipelineRun = pipelineRun;
        Role = role;
        Spark = spark;
        SparkJobDefinition = sparkJobDefinition;
        Sql = sql;
        SqlScript = sqlScript;
        Trigger = trigger;
        TriggerRun = triggerRun;
        Workspace = workspace;
        WorkspacePackage = workspacePackage;
    }

    public AzSynapseActivityRun ActivityRun { get; }

    public AzSynapseAdOnlyAuth AdOnlyAuth { get; }

    public AzSynapseDataFlow DataFlow { get; }

    public AzSynapseDataset Dataset { get; }

    public AzSynapseIntegrationRuntime IntegrationRuntime { get; }

    public AzSynapseIntegrationRuntimeNode IntegrationRuntimeNode { get; }

    public AzSynapseKqlScript KqlScript { get; }

    public AzSynapseKusto Kusto { get; }

    public AzSynapseKustoOperation KustoOperation { get; }

    public AzSynapseLinkConnection LinkConnection { get; }

    public AzSynapseLinkedService LinkedService { get; }

    public AzSynapseManagedPrivateEndpoints ManagedPrivateEndpoints { get; }

    public AzSynapseNotebook Notebook { get; }

    public AzSynapsePipeline Pipeline { get; }

    public AzSynapsePipelineRun PipelineRun { get; }

    public AzSynapseRole Role { get; }

    public AzSynapseSpark Spark { get; }

    public AzSynapseSparkJobDefinition SparkJobDefinition { get; }

    public AzSynapseSql Sql { get; }

    public AzSynapseSqlScript SqlScript { get; }

    public AzSynapseTrigger Trigger { get; }

    public AzSynapseTriggerRun TriggerRun { get; }

    public AzSynapseWorkspace Workspace { get; }

    public AzSynapseWorkspacePackage WorkspacePackage { get; }
}