using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzMl
{
    public AzMl(
        AzMlBatchDeployment batchDeployment,
        AzMlBatchEndpoint batchEndpoint,
        AzMlComponent component,
        AzMlCompute compute,
        AzMlComputetarget computetarget,
        AzMlConnection connection,
        AzMlData data,
        AzMlDataset dataset,
        AzMlDatastore datastore,
        AzMlEndpoint endpoint,
        AzMlEnvironment environment,
        AzMlExperiment experiment,
        AzMlFeatureSet featureSet,
        AzMlFeatureStore featureStore,
        AzMlFeatureStoreEntity featureStoreEntity,
        AzMlFolder folder,
        AzMlJob job,
        AzMlModel model,
        AzMlOnlineDeployment onlineDeployment,
        AzMlOnlineEndpoint onlineEndpoint,
        AzMlPipeline pipeline,
        AzMlRegistry registry,
        AzMlRun run,
        AzMlSchedule schedule,
        AzMlService service,
        AzMlWorkspace workspace,
        AzMlWorkspaceHub workspaceHub
    )
    {
        BatchDeployment = batchDeployment;
        BatchEndpoint = batchEndpoint;
        Component = component;
        Compute = compute;
        Computetarget = computetarget;
        Connection = connection;
        Data = data;
        Dataset = dataset;
        Datastore = datastore;
        Endpoint = endpoint;
        Environment = environment;
        Experiment = experiment;
        FeatureSet = featureSet;
        FeatureStore = featureStore;
        FeatureStoreEntity = featureStoreEntity;
        Folder = folder;
        Job = job;
        Model = model;
        OnlineDeployment = onlineDeployment;
        OnlineEndpoint = onlineEndpoint;
        Pipeline = pipeline;
        Registry = registry;
        Run = run;
        Schedule = schedule;
        Service = service;
        Workspace = workspace;
        WorkspaceHub = workspaceHub;
    }

    public AzMlBatchDeployment BatchDeployment { get; }

    public AzMlBatchEndpoint BatchEndpoint { get; }

    public AzMlComponent Component { get; }

    public AzMlCompute Compute { get; }

    public AzMlComputetarget Computetarget { get; }

    public AzMlConnection Connection { get; }

    public AzMlData Data { get; }

    public AzMlDataset Dataset { get; }

    public AzMlDatastore Datastore { get; }

    public AzMlEndpoint Endpoint { get; }

    public AzMlEnvironment Environment { get; }

    public AzMlExperiment Experiment { get; }

    public AzMlFeatureSet FeatureSet { get; }

    public AzMlFeatureStore FeatureStore { get; }

    public AzMlFeatureStoreEntity FeatureStoreEntity { get; }

    public AzMlFolder Folder { get; }

    public AzMlJob Job { get; }

    public AzMlModel Model { get; }

    public AzMlOnlineDeployment OnlineDeployment { get; }

    public AzMlOnlineEndpoint OnlineEndpoint { get; }

    public AzMlPipeline Pipeline { get; }

    public AzMlRegistry Registry { get; }

    public AzMlRun Run { get; }

    public AzMlSchedule Schedule { get; }

    public AzMlService Service { get; }

    public AzMlWorkspace Workspace { get; }

    public AzMlWorkspaceHub WorkspaceHub { get; }
}

