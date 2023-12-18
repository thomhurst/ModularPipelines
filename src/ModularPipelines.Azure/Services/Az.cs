using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class Az
{
    public Az(
        AzAccount account,
        AzAcr acr,
        AzAd ad,
        AzAdp adp,
        AzAdvisor advisor,
        AzAfd afd,
        AzAiExamples aiExamples,
        AzAks aks,
        AzAksarc aksarc,
        AzAkshybrid akshybrid,
        AzAlertsManagement alertsManagement,
        AzAlias alias,
        AzAmlfs amlfs,
        AzAms ams,
        AzAosm aosm,
        AzApic apic,
        AzApim apim,
        AzAppconfig appconfig,
        AzAppservice appservice,
        AzArcappliance arcappliance,
        AzArcdata arcdata,
        AzAro aro,
        AzArtifacts artifacts,
        AzAttestation attestation,
        AzAutomanage automanage,
        AzAutomation automation,
        AzAzurestackhci azurestackhci,
        AzBackup backup,
        AzBaremetalinstance baremetalinstance,
        AzBatch batch,
        AzBatchai batchai,
        AzBicep bicep,
        AzBilling billing,
        AzBillingBenefits billingBenefits,
        AzBlockchain blockchain,
        AzBlueprint blueprint,
        AzBoards boards,
        AzBot bot,
        AzCache cache,
        AzCapacity capacity,
        AzCdn cdn,
        AzChangeAnalysis changeAnalysis,
        AzCliTranslator cliTranslator,
        AzCloud cloud,
        AzCloudService cloudService,
        AzCognitiveservices cognitiveservices,
        AzCommandChange commandChange,
        AzCommunication communication,
        AzConfcom confcom,
        AzConfidentialledger confidentialledger,
        AzConfig config,
        AzConfluent confluent,
        AzConnectedk8s connectedk8s,
        AzConnectedmachine connectedmachine,
        AzConnectedvmware connectedvmware,
        AzConnection connection,
        AzConsumption consumption,
        AzContainer container,
        AzContainerapp containerapp,
        AzCosmosdb cosmosdb,
        AzCostmanagement costmanagement,
        AzCsvmware csvmware,
        AzCustomProviders customProviders,
        AzCustomlocation customlocation,
        AzDatabox databox,
        AzDataboxedge databoxedge,
        AzDatabricks databricks,
        AzDatadog datadog,
        AzDatafactory datafactory,
        AzDatamigration datamigration,
        AzDataprotection dataprotection,
        AzDatashare datashare,
        AzDedicatedHsm dedicatedHsm,
        AzDemo demo,
        AzDeployment deployment,
        AzDeploymentScripts deploymentScripts,
        AzDesktopvirtualization desktopvirtualization,
        AzDevcenter devcenter,
        AzDevops devops,
        AzDisk disk,
        AzDiskAccess diskAccess,
        AzDiskEncryptionSet diskEncryptionSet,
        AzDiskPool diskPool,
        AzDla dla,
        AzDls dls,
        AzDms dms,
        AzDnc dnc,
        AzDnsResolver dnsResolver,
        AzDt dt,
        AzDynatrace dynatrace,
        AzEdgeorder edgeorder,
        AzElastic elastic,
        AzElasticSan elasticSan,
        AzEventgrid eventgrid,
        AzEventhubs eventhubs,
        AzExtension extension,
        AzFeature feature,
        AzFleet fleet,
        AzFluidRelay fluidRelay,
        AzFootprint footprint,
        AzFunctionapp functionapp,
        AzFzf fzf,
        AzGrafana grafana,
        AzGraph graph,
        AzGraphServices graphServices,
        AzGroup group,
        AzGuestconfig guestconfig,
        AzHack hack,
        AzHanainstance hanainstance,
        AzHdinsight hdinsight,
        AzHdinsightOnAks hdinsightOnAks,
        AzHealthbot healthbot,
        AzHealthcareapis healthcareapis,
        AzHpcCache hpcCache,
        AzHybridaks hybridaks,
        AzIdentity identity,
        AzImage image,
        AzImportExport importExport,
        AzInternetAnalyzer internetAnalyzer,
        AzIot iot,
        AzK8sExtension k8sExtension,
        AzK8sconfiguration k8sconfiguration,
        AzKeyvault keyvault,
        AzKusto kusto,
        AzLab lab,
        AzLargeInstance largeInstance,
        AzLargeStorageInstance largeStorageInstance,
        AzLoad load,
        AzLock @lock,
        AzLogic logic,
        AzLogicapp logicapp,
        AzLogz logz,
        AzMaintenance maintenance,
        AzManagedCassandra managedCassandra,
        AzManagedapp managedapp,
        AzManagedservices managedservices,
        AzManagementpartner managementpartner,
        AzMaps maps,
        AzMariadb mariadb,
        AzMesh mesh,
        AzMl ml,
        AzMobileNetwork mobileNetwork,
        AzMonitor monitor,
        AzMysql mysql,
        AzNetappfiles netappfiles,
        AzNetwork network,
        AzNetworkAnalytics networkAnalytics,
        AzNetworkFunction networkFunction,
        AzNetworkcloud networkcloud,
        AzNetworkfabric networkfabric,
        AzNewRelic newRelic,
        AzNginx nginx,
        AzNotificationHub notificationHub,
        AzOffazure offazure,
        AzOrbital orbital,
        AzPaloAlto paloAlto,
        AzPartnercenter partnercenter,
        AzPeering peering,
        AzPipelines pipelines,
        AzPolicy policy,
        AzPortal portal,
        AzPostgres postgres,
        AzPowerbi powerbi,
        AzPpg ppg,
        AzPrivateLink privateLink,
        AzProvider provider,
        AzProviderhub providerhub,
        AzPurview purview,
        AzQuantum quantum,
        AzQumulo qumulo,
        AzQuota quota,
        AzRedis redis,
        AzRedisenterprise redisenterprise,
        AzRelay relay,
        AzRemoteRenderingAccount remoteRenderingAccount,
        AzRepos repos,
        AzReservations reservations,
        AzResource resource,
        AzResourceMover resourceMover,
        AzResourcemanagement resourcemanagement,
        AzRestorePoint restorePoint,
        AzRole role,
        AzSapmonitor sapmonitor,
        AzScenario scenario,
        AzScvmm scvmm,
        AzSearch search,
        AzSecurity security,
        AzSelfHelp selfHelp,
        AzSentinel sentinel,
        AzSerialConsole serialConsole,
        AzServicebus servicebus,
        AzSf sf,
        AzSig sig,
        AzSignalr signalr,
        AzSiteRecovery siteRecovery,
        AzSnapshot snapshot,
        AzSpatialAnchorsAccount spatialAnchorsAccount,
        AzSphere sphere,
        AzSpring spring,
        AzSpringCloud springCloud,
        AzSql sql,
        AzSsh ssh,
        AzSshkey sshkey,
        AzStack stack,
        AzStackHci stackHci,
        AzStackHciVm stackHciVm,
        AzStaticwebapp staticwebapp,
        AzStorage storage,
        AzStorageMover storageMover,
        AzStoragesync storagesync,
        AzStreamAnalytics streamAnalytics,
        AzSupport support,
        AzSynapse synapse,
        AzTag tag,
        AzTerm term,
        AzTs ts,
        AzTsi tsi,
        AzVm vm,
        AzVmss vmss,
        AzVmware vmware,
        AzWebapp webapp,
        AzWebpubsub webpubsub,
        AzWorkloads workloads,
        ICommand internalCommand
    )
    {
        Account = account;
        Acr = acr;
        Ad = ad;
        Adp = adp;
        Advisor = advisor;
        Afd = afd;
        AiExamples = aiExamples;
        Aks = aks;
        Aksarc = aksarc;
        Akshybrid = akshybrid;
        AlertsManagement = alertsManagement;
        Alias = alias;
        Amlfs = amlfs;
        Ams = ams;
        Aosm = aosm;
        Apic = apic;
        Apim = apim;
        Appconfig = appconfig;
        Appservice = appservice;
        Arcappliance = arcappliance;
        Arcdata = arcdata;
        Aro = aro;
        Artifacts = artifacts;
        Attestation = attestation;
        Automanage = automanage;
        Automation = automation;
        Azurestackhci = azurestackhci;
        Backup = backup;
        Baremetalinstance = baremetalinstance;
        Batch = batch;
        Batchai = batchai;
        Bicep = bicep;
        Billing = billing;
        BillingBenefits = billingBenefits;
        Blockchain = blockchain;
        Blueprint = blueprint;
        Boards = boards;
        Bot = bot;
        Cache = cache;
        Capacity = capacity;
        Cdn = cdn;
        ChangeAnalysis = changeAnalysis;
        CliTranslator = cliTranslator;
        Cloud = cloud;
        CloudService = cloudService;
        Cognitiveservices = cognitiveservices;
        CommandChange = commandChange;
        Communication = communication;
        Confcom = confcom;
        Confidentialledger = confidentialledger;
        Config = config;
        Confluent = confluent;
        Connectedk8s = connectedk8s;
        Connectedmachine = connectedmachine;
        Connectedvmware = connectedvmware;
        Connection = connection;
        Consumption = consumption;
        Container = container;
        Containerapp = containerapp;
        Cosmosdb = cosmosdb;
        Costmanagement = costmanagement;
        Csvmware = csvmware;
        CustomProviders = customProviders;
        Customlocation = customlocation;
        Databox = databox;
        Databoxedge = databoxedge;
        Databricks = databricks;
        Datadog = datadog;
        Datafactory = datafactory;
        Datamigration = datamigration;
        Dataprotection = dataprotection;
        Datashare = datashare;
        DedicatedHsm = dedicatedHsm;
        Demo = demo;
        Deployment = deployment;
        DeploymentScripts = deploymentScripts;
        Desktopvirtualization = desktopvirtualization;
        Devcenter = devcenter;
        Devops = devops;
        Disk = disk;
        DiskAccess = diskAccess;
        DiskEncryptionSet = diskEncryptionSet;
        DiskPool = diskPool;
        Dla = dla;
        Dls = dls;
        Dms = dms;
        Dnc = dnc;
        DnsResolver = dnsResolver;
        Dt = dt;
        Dynatrace = dynatrace;
        Edgeorder = edgeorder;
        Elastic = elastic;
        ElasticSan = elasticSan;
        Eventgrid = eventgrid;
        Eventhubs = eventhubs;
        Extension = extension;
        Feature = feature;
        Fleet = fleet;
        FluidRelay = fluidRelay;
        Footprint = footprint;
        Functionapp = functionapp;
        Fzf = fzf;
        Grafana = grafana;
        Graph = graph;
        GraphServices = graphServices;
        Group = group;
        Guestconfig = guestconfig;
        Hack = hack;
        Hanainstance = hanainstance;
        Hdinsight = hdinsight;
        HdinsightOnAks = hdinsightOnAks;
        Healthbot = healthbot;
        Healthcareapis = healthcareapis;
        HpcCache = hpcCache;
        Hybridaks = hybridaks;
        Identity = identity;
        Image = image;
        ImportExport = importExport;
        InternetAnalyzer = internetAnalyzer;
        Iot = iot;
        K8sExtension = k8sExtension;
        K8sconfiguration = k8sconfiguration;
        Keyvault = keyvault;
        Kusto = kusto;
        Lab = lab;
        LargeInstance = largeInstance;
        LargeStorageInstance = largeStorageInstance;
        Load = load;
        Lock = @lock;
        Logic = logic;
        Logicapp = logicapp;
        Logz = logz;
        Maintenance = maintenance;
        ManagedCassandra = managedCassandra;
        Managedapp = managedapp;
        Managedservices = managedservices;
        Managementpartner = managementpartner;
        Maps = maps;
        Mariadb = mariadb;
        Mesh = mesh;
        Ml = ml;
        MobileNetwork = mobileNetwork;
        Monitor = monitor;
        Mysql = mysql;
        Netappfiles = netappfiles;
        Network = network;
        NetworkAnalytics = networkAnalytics;
        NetworkFunction = networkFunction;
        Networkcloud = networkcloud;
        Networkfabric = networkfabric;
        NewRelic = newRelic;
        Nginx = nginx;
        NotificationHub = notificationHub;
        Offazure = offazure;
        Orbital = orbital;
        PaloAlto = paloAlto;
        Partnercenter = partnercenter;
        Peering = peering;
        Pipelines = pipelines;
        Policy = policy;
        Portal = portal;
        Postgres = postgres;
        Powerbi = powerbi;
        Ppg = ppg;
        PrivateLink = privateLink;
        Provider = provider;
        Providerhub = providerhub;
        Purview = purview;
        Quantum = quantum;
        Qumulo = qumulo;
        Quota = quota;
        Redis = redis;
        Redisenterprise = redisenterprise;
        Relay = relay;
        RemoteRenderingAccount = remoteRenderingAccount;
        Repos = repos;
        Reservations = reservations;
        Resource = resource;
        ResourceMover = resourceMover;
        Resourcemanagement = resourcemanagement;
        RestorePoint = restorePoint;
        Role = role;
        Sapmonitor = sapmonitor;
        Scenario = scenario;
        Scvmm = scvmm;
        Search = search;
        Security = security;
        SelfHelp = selfHelp;
        Sentinel = sentinel;
        SerialConsole = serialConsole;
        Servicebus = servicebus;
        Sf = sf;
        Sig = sig;
        Signalr = signalr;
        SiteRecovery = siteRecovery;
        Snapshot = snapshot;
        SpatialAnchorsAccount = spatialAnchorsAccount;
        Sphere = sphere;
        Spring = spring;
        SpringCloud = springCloud;
        Sql = sql;
        Ssh = ssh;
        Sshkey = sshkey;
        Stack = stack;
        StackHci = stackHci;
        StackHciVm = stackHciVm;
        Staticwebapp = staticwebapp;
        Storage = storage;
        StorageMover = storageMover;
        Storagesync = storagesync;
        StreamAnalytics = streamAnalytics;
        Support = support;
        Synapse = synapse;
        Tag = tag;
        Term = term;
        Ts = ts;
        Tsi = tsi;
        Vm = vm;
        Vmss = vmss;
        Vmware = vmware;
        Webapp = webapp;
        Webpubsub = webpubsub;
        Workloads = workloads;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAccount Account { get; }

    public AzAcr Acr { get; }

    public AzAd Ad { get; }

    public AzAdp Adp { get; }

    public AzAdvisor Advisor { get; }

    public AzAfd Afd { get; }

    public AzAiExamples AiExamples { get; }

    public AzAks Aks { get; }

    public AzAksarc Aksarc { get; }

    public AzAkshybrid Akshybrid { get; }

    public AzAlertsManagement AlertsManagement { get; }

    public AzAlias Alias { get; }

    public AzAmlfs Amlfs { get; }

    public AzAms Ams { get; }

    public AzAosm Aosm { get; }

    public AzApic Apic { get; }

    public AzApim Apim { get; }

    public AzAppconfig Appconfig { get; }

    public AzAppservice Appservice { get; }

    public AzArcappliance Arcappliance { get; }

    public AzArcdata Arcdata { get; }

    public AzAro Aro { get; }

    public AzArtifacts Artifacts { get; }

    public AzAttestation Attestation { get; }

    public AzAutomanage Automanage { get; }

    public AzAutomation Automation { get; }

    public AzAzurestackhci Azurestackhci { get; }

    public AzBackup Backup { get; }

    public AzBaremetalinstance Baremetalinstance { get; }

    public AzBatch Batch { get; }

    public AzBatchai Batchai { get; }

    public AzBicep Bicep { get; }

    public AzBilling Billing { get; }

    public AzBillingBenefits BillingBenefits { get; }

    public AzBlockchain Blockchain { get; }

    public AzBlueprint Blueprint { get; }

    public AzBoards Boards { get; }

    public AzBot Bot { get; }

    public AzCache Cache { get; }

    public AzCapacity Capacity { get; }

    public AzCdn Cdn { get; }

    public AzChangeAnalysis ChangeAnalysis { get; }

    public AzCliTranslator CliTranslator { get; }

    public AzCloud Cloud { get; }

    public AzCloudService CloudService { get; }

    public AzCognitiveservices Cognitiveservices { get; }

    public AzCommandChange CommandChange { get; }

    public AzCommunication Communication { get; }

    public AzConfcom Confcom { get; }

    public AzConfidentialledger Confidentialledger { get; }

    public AzConfig Config { get; }

    public AzConfluent Confluent { get; }

    public AzConnectedk8s Connectedk8s { get; }

    public AzConnectedmachine Connectedmachine { get; }

    public AzConnectedvmware Connectedvmware { get; }

    public AzConnection Connection { get; }

    public AzConsumption Consumption { get; }

    public AzContainer Container { get; }

    public AzContainerapp Containerapp { get; }

    public AzCosmosdb Cosmosdb { get; }

    public AzCostmanagement Costmanagement { get; }

    public AzCsvmware Csvmware { get; }

    public AzCustomProviders CustomProviders { get; }

    public AzCustomlocation Customlocation { get; }

    public AzDatabox Databox { get; }

    public AzDataboxedge Databoxedge { get; }

    public AzDatabricks Databricks { get; }

    public AzDatadog Datadog { get; }

    public AzDatafactory Datafactory { get; }

    public AzDatamigration Datamigration { get; }

    public AzDataprotection Dataprotection { get; }

    public AzDatashare Datashare { get; }

    public AzDedicatedHsm DedicatedHsm { get; }

    public AzDemo Demo { get; }

    public AzDeployment Deployment { get; }

    public AzDeploymentScripts DeploymentScripts { get; }

    public AzDesktopvirtualization Desktopvirtualization { get; }

    public AzDevcenter Devcenter { get; }

    public AzDevops Devops { get; }

    public AzDisk Disk { get; }

    public AzDiskAccess DiskAccess { get; }

    public AzDiskEncryptionSet DiskEncryptionSet { get; }

    public AzDiskPool DiskPool { get; }

    public AzDla Dla { get; }

    public AzDls Dls { get; }

    public AzDms Dms { get; }

    public AzDnc Dnc { get; }

    public AzDnsResolver DnsResolver { get; }

    public AzDt Dt { get; }

    public AzDynatrace Dynatrace { get; }

    public AzEdgeorder Edgeorder { get; }

    public AzElastic Elastic { get; }

    public AzElasticSan ElasticSan { get; }

    public AzEventgrid Eventgrid { get; }

    public AzEventhubs Eventhubs { get; }

    public AzExtension Extension { get; }

    public AzFeature Feature { get; }

    public AzFleet Fleet { get; }

    public AzFluidRelay FluidRelay { get; }

    public AzFootprint Footprint { get; }

    public AzFunctionapp Functionapp { get; }

    public AzFzf Fzf { get; }

    public AzGrafana Grafana { get; }

    public AzGraph Graph { get; }

    public AzGraphServices GraphServices { get; }

    public AzGroup Group { get; }

    public AzGuestconfig Guestconfig { get; }

    public AzHack Hack { get; }

    public AzHanainstance Hanainstance { get; }

    public AzHdinsight Hdinsight { get; }

    public AzHdinsightOnAks HdinsightOnAks { get; }

    public AzHealthbot Healthbot { get; }

    public AzHealthcareapis Healthcareapis { get; }

    public AzHpcCache HpcCache { get; }

    public AzHybridaks Hybridaks { get; }

    public AzIdentity Identity { get; }

    public AzImage Image { get; }

    public AzImportExport ImportExport { get; }

    public AzInternetAnalyzer InternetAnalyzer { get; }

    public AzIot Iot { get; }

    public AzK8sExtension K8sExtension { get; }

    public AzK8sconfiguration K8sconfiguration { get; }

    public AzKeyvault Keyvault { get; }

    public AzKusto Kusto { get; }

    public AzLab Lab { get; }

    public AzLargeInstance LargeInstance { get; }

    public AzLargeStorageInstance LargeStorageInstance { get; }

    public AzLoad Load { get; }

    public AzLock Lock { get; }

    public AzLogic Logic { get; }

    public AzLogicapp Logicapp { get; }

    public AzLogz Logz { get; }

    public AzMaintenance Maintenance { get; }

    public AzManagedCassandra ManagedCassandra { get; }

    public AzManagedapp Managedapp { get; }

    public AzManagedservices Managedservices { get; }

    public AzManagementpartner Managementpartner { get; }

    public AzMaps Maps { get; }

    public AzMariadb Mariadb { get; }

    public AzMesh Mesh { get; }

    public AzMl Ml { get; }

    public AzMobileNetwork MobileNetwork { get; }

    public AzMonitor Monitor { get; }

    public AzMysql Mysql { get; }

    public AzNetappfiles Netappfiles { get; }

    public AzNetwork Network { get; }

    public AzNetworkAnalytics NetworkAnalytics { get; }

    public AzNetworkFunction NetworkFunction { get; }

    public AzNetworkcloud Networkcloud { get; }

    public AzNetworkfabric Networkfabric { get; }

    public AzNewRelic NewRelic { get; }

    public AzNginx Nginx { get; }

    public AzNotificationHub NotificationHub { get; }

    public AzOffazure Offazure { get; }

    public AzOrbital Orbital { get; }

    public AzPaloAlto PaloAlto { get; }

    public AzPartnercenter Partnercenter { get; }

    public AzPeering Peering { get; }

    public AzPipelines Pipelines { get; }

    public AzPolicy Policy { get; }

    public AzPortal Portal { get; }

    public AzPostgres Postgres { get; }

    public AzPowerbi Powerbi { get; }

    public AzPpg Ppg { get; }

    public AzPrivateLink PrivateLink { get; }

    public AzProvider Provider { get; }

    public AzProviderhub Providerhub { get; }

    public AzPurview Purview { get; }

    public AzQuantum Quantum { get; }

    public AzQumulo Qumulo { get; }

    public AzQuota Quota { get; }

    public AzRedis Redis { get; }

    public AzRedisenterprise Redisenterprise { get; }

    public AzRelay Relay { get; }

    public AzRemoteRenderingAccount RemoteRenderingAccount { get; }

    public AzRepos Repos { get; }

    public AzReservations Reservations { get; }

    public AzResource Resource { get; }

    public AzResourceMover ResourceMover { get; }

    public AzResourcemanagement Resourcemanagement { get; }

    public AzRestorePoint RestorePoint { get; }

    public AzRole Role { get; }

    public AzSapmonitor Sapmonitor { get; }

    public AzScenario Scenario { get; }

    public AzScvmm Scvmm { get; }

    public AzSearch Search { get; }

    public AzSecurity Security { get; }

    public AzSelfHelp SelfHelp { get; }

    public AzSentinel Sentinel { get; }

    public AzSerialConsole SerialConsole { get; }

    public AzServicebus Servicebus { get; }

    public AzSf Sf { get; }

    public AzSig Sig { get; }

    public AzSignalr Signalr { get; }

    public AzSiteRecovery SiteRecovery { get; }

    public AzSnapshot Snapshot { get; }

    public AzSpatialAnchorsAccount SpatialAnchorsAccount { get; }

    public AzSphere Sphere { get; }

    public AzSpring Spring { get; }

    public AzSpringCloud SpringCloud { get; }

    public AzSql Sql { get; }

    public AzSsh Ssh { get; }

    public AzSshkey Sshkey { get; }

    public AzStack Stack { get; }

    public AzStackHci StackHci { get; }

    public AzStackHciVm StackHciVm { get; }

    public AzStaticwebapp Staticwebapp { get; }

    public AzStorage Storage { get; }

    public AzStorageMover StorageMover { get; }

    public AzStoragesync Storagesync { get; }

    public AzStreamAnalytics StreamAnalytics { get; }

    public AzSupport Support { get; }

    public AzSynapse Synapse { get; }

    public AzTag Tag { get; }

    public AzTerm Term { get; }

    public AzTs Ts { get; }

    public AzTsi Tsi { get; }

    public AzVm Vm { get; }

    public AzVmss Vmss { get; }

    public AzVmware Vmware { get; }

    public AzWebapp Webapp { get; }

    public AzWebpubsub Webpubsub { get; }

    public AzWorkloads Workloads { get; }

    public async Task<CommandResult> Configure(AzConfigureOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Feedback(AzFeedbackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Find(AzFindOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Init(AzInitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Interactive(AzInteractiveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Login(AzLoginOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Logout(AzLogoutOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Next(AzNextOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Rest(AzRestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SelfTest(AzSelfTestOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSelfTestOptions(), token);
    }

    public async Task<CommandResult> Survey(AzSurveyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSurveyOptions(), token);
    }

    public async Task<CommandResult> Upgrade(AzUpgradeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzUpgradeOptions(), token);
    }

    public async Task<CommandResult> Version(AzVersionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVersionOptions(), token);
    }
}