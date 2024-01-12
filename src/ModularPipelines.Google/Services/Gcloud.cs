using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class Gcloud
{
    public Gcloud(
        GcloudAccessApproval accessApproval,
        GcloudAccessContextManager accessContextManager,
        GcloudActiveDirectory activeDirectory,
        GcloudAi ai,
        GcloudAiPlatform aiPlatform,
        GcloudAlloydb alloydb,
        GcloudAnthos anthos,
        GcloudApiGateway apiGateway,
        GcloudApigee apigee,
        GcloudApp app,
        GcloudArtifacts artifacts,
        GcloudAsset asset,
        GcloudAssured assured,
        GcloudAuth auth,
        GcloudBackupDr backupDr,
        GcloudBatch batch,
        GcloudBigtable bigtable,
        GcloudBilling billing,
        GcloudBms bms,
        GcloudBuilds builds,
        GcloudCertificateManager certificateManager,
        GcloudCloudShell cloudShell,
        GcloudComponents components,
        GcloudComposer composer,
        GcloudCompute compute,
        GcloudConfig config,
        GcloudContainer container,
        GcloudDataCatalog dataCatalog,
        GcloudDatabaseMigration databaseMigration,
        GcloudDataflow dataflow,
        GcloudDataplex dataplex,
        GcloudDataproc dataproc,
        GcloudDatastore datastore,
        GcloudDatastream datastream,
        GcloudDeploy deploy,
        GcloudDeploymentManager deploymentManager,
        GcloudDns dns,
        GcloudDomains domains,
        GcloudEdgeCache edgeCache,
        GcloudEdgeCloud edgeCloud,
        GcloudEmulators emulators,
        GcloudEndpoints endpoints,
        GcloudEssentialContacts essentialContacts,
        GcloudEventarc eventarc,
        GcloudFilestore filestore,
        GcloudFirebase firebase,
        GcloudFirestore firestore,
        GcloudFunctions functions,
        GcloudHealthcare healthcare,
        GcloudIam iam,
        GcloudIap iap,
        GcloudIdentity identity,
        GcloudIds ids,
        GcloudImmersiveStream immersiveStream,
        GcloudInfraManager infraManager,
        GcloudKms kms,
        GcloudLogging logging,
        GcloudLooker looker,
        GcloudMemcache memcache,
        GcloudMetastore metastore,
        GcloudMl ml,
        GcloudMlEngine mlEngine,
        GcloudMonitoring monitoring,
        GcloudNetapp netapp,
        GcloudNetworkConnectivity networkConnectivity,
        GcloudNetworkManagement networkManagement,
        GcloudNetworkSecurity networkSecurity,
        GcloudNetworkServices networkServices,
        GcloudNotebooks notebooks,
        GcloudOrgPolicies orgPolicies,
        GcloudOrganizations organizations,
        GcloudPolicyIntelligence policyIntelligence,
        GcloudPolicyTroubleshoot policyTroubleshoot,
        GcloudPrivateca privateca,
        GcloudProjects projects,
        GcloudPublicca publicca,
        GcloudPubsub pubsub,
        GcloudRecaptcha recaptcha,
        GcloudRecommender recommender,
        GcloudRedis redis,
        GcloudResourceManager resourceManager,
        GcloudResourceSettings resourceSettings,
        GcloudRun run,
        GcloudScc scc,
        GcloudScheduler scheduler,
        GcloudSecrets secrets,
        GcloudServiceDirectory serviceDirectory,
        GcloudServices services,
        GcloudSource source,
        GcloudSpanner spanner,
        GcloudSql sql,
        GcloudStorage storage,
        GcloudTasks tasks,
        GcloudTelcoAutomation telcoAutomation,
        GcloudTopic topic,
        GcloudTranscoder transcoder,
        GcloudTransfer transfer,
        GcloudVmware vmware,
        GcloudWorkbench workbench,
        GcloudWorkflows workflows,
        GcloudWorkspaceAddOns workspaceAddOns,
        GcloudWorkstations workstations,
        ICommand internalCommand
    )
    {
        AccessApproval = accessApproval;
        AccessContextManager = accessContextManager;
        ActiveDirectory = activeDirectory;
        Ai = ai;
        AiPlatform = aiPlatform;
        Alloydb = alloydb;
        Anthos = anthos;
        ApiGateway = apiGateway;
        Apigee = apigee;
        App = app;
        Artifacts = artifacts;
        Asset = asset;
        Assured = assured;
        Auth = auth;
        BackupDr = backupDr;
        Batch = batch;
        Bigtable = bigtable;
        Billing = billing;
        Bms = bms;
        Builds = builds;
        CertificateManager = certificateManager;
        CloudShell = cloudShell;
        Components = components;
        Composer = composer;
        Compute = compute;
        Config = config;
        Container = container;
        DataCatalog = dataCatalog;
        DatabaseMigration = databaseMigration;
        Dataflow = dataflow;
        Dataplex = dataplex;
        Dataproc = dataproc;
        Datastore = datastore;
        Datastream = datastream;
        Deploy = deploy;
        DeploymentManager = deploymentManager;
        Dns = dns;
        Domains = domains;
        EdgeCache = edgeCache;
        EdgeCloud = edgeCloud;
        Emulators = emulators;
        Endpoints = endpoints;
        EssentialContacts = essentialContacts;
        Eventarc = eventarc;
        Filestore = filestore;
        Firebase = firebase;
        Firestore = firestore;
        Functions = functions;
        Healthcare = healthcare;
        Iam = iam;
        Iap = iap;
        Identity = identity;
        Ids = ids;
        ImmersiveStream = immersiveStream;
        InfraManager = infraManager;
        Kms = kms;
        Logging = logging;
        Looker = looker;
        Memcache = memcache;
        Metastore = metastore;
        Ml = ml;
        MlEngine = mlEngine;
        Monitoring = monitoring;
        Netapp = netapp;
        NetworkConnectivity = networkConnectivity;
        NetworkManagement = networkManagement;
        NetworkSecurity = networkSecurity;
        NetworkServices = networkServices;
        Notebooks = notebooks;
        OrgPolicies = orgPolicies;
        Organizations = organizations;
        PolicyIntelligence = policyIntelligence;
        PolicyTroubleshoot = policyTroubleshoot;
        Privateca = privateca;
        Projects = projects;
        Publicca = publicca;
        Pubsub = pubsub;
        Recaptcha = recaptcha;
        Recommender = recommender;
        Redis = redis;
        ResourceManager = resourceManager;
        ResourceSettings = resourceSettings;
        Run = run;
        Scc = scc;
        Scheduler = scheduler;
        Secrets = secrets;
        ServiceDirectory = serviceDirectory;
        Services = services;
        Source = source;
        Spanner = spanner;
        Sql = sql;
        Storage = storage;
        Tasks = tasks;
        TelcoAutomation = telcoAutomation;
        Topic = topic;
        Transcoder = transcoder;
        Transfer = transfer;
        Vmware = vmware;
        Workbench = workbench;
        Workflows = workflows;
        WorkspaceAddOns = workspaceAddOns;
        Workstations = workstations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudAccessApproval AccessApproval { get; }

    public GcloudAccessContextManager AccessContextManager { get; }

    public GcloudActiveDirectory ActiveDirectory { get; }

    public GcloudAi Ai { get; }

    public GcloudAiPlatform AiPlatform { get; }

    public GcloudAlloydb Alloydb { get; }

    public GcloudAnthos Anthos { get; }

    public GcloudApiGateway ApiGateway { get; }

    public GcloudApigee Apigee { get; }

    public GcloudApp App { get; }

    public GcloudArtifacts Artifacts { get; }

    public GcloudAsset Asset { get; }

    public GcloudAssured Assured { get; }

    public GcloudAuth Auth { get; }

    public GcloudBackupDr BackupDr { get; }

    public GcloudBatch Batch { get; }

    public GcloudBigtable Bigtable { get; }

    public GcloudBilling Billing { get; }

    public GcloudBms Bms { get; }

    public GcloudBuilds Builds { get; }

    public GcloudCertificateManager CertificateManager { get; }

    public GcloudCloudShell CloudShell { get; }

    public GcloudComponents Components { get; }

    public GcloudComposer Composer { get; }

    public GcloudCompute Compute { get; }

    public GcloudConfig Config { get; }

    public GcloudContainer Container { get; }

    public GcloudDataCatalog DataCatalog { get; }

    public GcloudDatabaseMigration DatabaseMigration { get; }

    public GcloudDataflow Dataflow { get; }

    public GcloudDataplex Dataplex { get; }

    public GcloudDataproc Dataproc { get; }

    public GcloudDatastore Datastore { get; }

    public GcloudDatastream Datastream { get; }

    public GcloudDeploy Deploy { get; }

    public GcloudDeploymentManager DeploymentManager { get; }

    public GcloudDns Dns { get; }

    public GcloudDomains Domains { get; }

    public GcloudEdgeCache EdgeCache { get; }

    public GcloudEdgeCloud EdgeCloud { get; }

    public GcloudEmulators Emulators { get; }

    public GcloudEndpoints Endpoints { get; }

    public GcloudEssentialContacts EssentialContacts { get; }

    public GcloudEventarc Eventarc { get; }

    public GcloudFilestore Filestore { get; }

    public GcloudFirebase Firebase { get; }

    public GcloudFirestore Firestore { get; }

    public GcloudFunctions Functions { get; }

    public GcloudHealthcare Healthcare { get; }

    public GcloudIam Iam { get; }

    public GcloudIap Iap { get; }

    public GcloudIdentity Identity { get; }

    public GcloudIds Ids { get; }

    public GcloudImmersiveStream ImmersiveStream { get; }

    public GcloudInfraManager InfraManager { get; }

    public GcloudKms Kms { get; }

    public GcloudLogging Logging { get; }

    public GcloudLooker Looker { get; }

    public GcloudMemcache Memcache { get; }

    public GcloudMetastore Metastore { get; }

    public GcloudMl Ml { get; }

    public GcloudMlEngine MlEngine { get; }

    public GcloudMonitoring Monitoring { get; }

    public GcloudNetapp Netapp { get; }

    public GcloudNetworkConnectivity NetworkConnectivity { get; }

    public GcloudNetworkManagement NetworkManagement { get; }

    public GcloudNetworkSecurity NetworkSecurity { get; }

    public GcloudNetworkServices NetworkServices { get; }

    public GcloudNotebooks Notebooks { get; }

    public GcloudOrgPolicies OrgPolicies { get; }

    public GcloudOrganizations Organizations { get; }

    public GcloudPolicyIntelligence PolicyIntelligence { get; }

    public GcloudPolicyTroubleshoot PolicyTroubleshoot { get; }

    public GcloudPrivateca Privateca { get; }

    public GcloudProjects Projects { get; }

    public GcloudPublicca Publicca { get; }

    public GcloudPubsub Pubsub { get; }

    public GcloudRecaptcha Recaptcha { get; }

    public GcloudRecommender Recommender { get; }

    public GcloudRedis Redis { get; }

    public GcloudResourceManager ResourceManager { get; }

    public GcloudResourceSettings ResourceSettings { get; }

    public GcloudRun Run { get; }

    public GcloudScc Scc { get; }

    public GcloudScheduler Scheduler { get; }

    public GcloudSecrets Secrets { get; }

    public GcloudServiceDirectory ServiceDirectory { get; }

    public GcloudServices Services { get; }

    public GcloudSource Source { get; }

    public GcloudSpanner Spanner { get; }

    public GcloudSql Sql { get; }

    public GcloudStorage Storage { get; }

    public GcloudTasks Tasks { get; }

    public GcloudTelcoAutomation TelcoAutomation { get; }

    public GcloudTopic Topic { get; }

    public GcloudTranscoder Transcoder { get; }

    public GcloudTransfer Transfer { get; }

    public GcloudVmware Vmware { get; }

    public GcloudWorkbench Workbench { get; }

    public GcloudWorkflows Workflows { get; }

    public GcloudWorkspaceAddOns WorkspaceAddOns { get; }

    public GcloudWorkstations Workstations { get; }

    public async Task<CommandResult> CheatSheet(GcloudCheatSheetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudCheatSheetOptions(), token);
    }

    public async Task<CommandResult> Feedback(GcloudFeedbackOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudFeedbackOptions(), token);
    }

    public async Task<CommandResult> Help(GcloudHelpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Info(GcloudInfoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudInfoOptions(), token);
    }

    public async Task<CommandResult> Init(GcloudInitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudInitOptions(), token);
    }

    public async Task<CommandResult> Survey(GcloudSurveyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudSurveyOptions(), token);
    }

    public async Task<CommandResult> Version(GcloudVersionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudVersionOptions(), token);
    }
}