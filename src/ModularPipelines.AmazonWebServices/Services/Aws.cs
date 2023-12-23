using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class Aws
{
    public Aws(
        AwsAccessanalyzer accessanalyzer,
        AwsAccount account,
        AwsAcm acm,
        AwsAcmPca acmPca,
        AwsAlexaforbusiness alexaforbusiness,
        AwsAmp amp,
        AwsAmplify amplify,
        AwsAmplifybackend amplifybackend,
        AwsAmplifyuibuilder amplifyuibuilder,
        AwsApigateway apigateway,
        AwsApigatewaymanagementapi apigatewaymanagementapi,
        AwsApigatewayv2 apigatewayv2,
        AwsAppconfig appconfig,
        AwsAppconfigdata appconfigdata,
        AwsAppfabric appfabric,
        AwsAppflow appflow,
        AwsAppintegrations appintegrations,
        AwsApplicationAutoscaling applicationAutoscaling,
        AwsApplicationInsights applicationInsights,
        AwsApplicationcostprofiler applicationcostprofiler,
        AwsAppmesh appmesh,
        AwsApprunner apprunner,
        AwsAppstream appstream,
        AwsAppsync appsync,
        AwsArcZonalShift arcZonalShift,
        AwsAthena athena,
        AwsAuditmanager auditmanager,
        AwsAutoscaling autoscaling,
        AwsAutoscalingPlans autoscalingPlans,
        AwsB2bi b2bi,
        AwsBackup backup,
        AwsBackupGateway backupGateway,
        AwsBackupstorage backupstorage,
        AwsBatch batch,
        AwsBcmDataExports bcmDataExports,
        AwsBedrock bedrock,
        AwsBedrockAgent bedrockAgent,
        AwsBedrockAgentRuntime bedrockAgentRuntime,
        AwsBedrockRuntime bedrockRuntime,
        AwsBillingconductor billingconductor,
        AwsBraket braket,
        AwsBudgets budgets,
        AwsCe ce,
        AwsChime chime,
        AwsChimeSdkIdentity chimeSdkIdentity,
        AwsChimeSdkMediaPipelines chimeSdkMediaPipelines,
        AwsChimeSdkMeetings chimeSdkMeetings,
        AwsChimeSdkMessaging chimeSdkMessaging,
        AwsChimeSdkVoice chimeSdkVoice,
        AwsCleanrooms cleanrooms,
        AwsCleanroomsml cleanroomsml,
        AwsCloud9 cloud9,
        AwsCloudcontrol cloudcontrol,
        AwsClouddirectory clouddirectory,
        AwsCloudformation cloudformation,
        AwsCloudfront cloudfront,
        AwsCloudfrontKeyvaluestore cloudfrontKeyvaluestore,
        AwsCloudhsmv2 cloudhsmv2,
        AwsCloudsearch cloudsearch,
        AwsCloudsearchdomain cloudsearchdomain,
        AwsCloudtrail cloudtrail,
        AwsCloudtrailData cloudtrailData,
        AwsCloudwatch cloudwatch,
        AwsCodeartifact codeartifact,
        AwsCodebuild codebuild,
        AwsCodecatalyst codecatalyst,
        AwsCodecommit codecommit,
        AwsCodeguruReviewer codeguruReviewer,
        AwsCodeguruSecurity codeguruSecurity,
        AwsCodeguruprofiler codeguruprofiler,
        AwsCodepipeline codepipeline,
        AwsCodestar codestar,
        AwsCodestarConnections codestarConnections,
        AwsCodestarNotifications codestarNotifications,
        AwsCognitoIdentity cognitoIdentity,
        AwsCognitoIdp cognitoIdp,
        AwsCognitoSync cognitoSync,
        AwsComprehend comprehend,
        AwsComprehendmedical comprehendmedical,
        AwsComputeOptimizer computeOptimizer,
        AwsConfigservice configservice,
        AwsConnect connect,
        AwsConnectContactLens connectContactLens,
        AwsConnectcampaigns connectcampaigns,
        AwsConnectcases connectcases,
        AwsConnectparticipant connectparticipant,
        AwsControltower controltower,
        AwsCostOptimizationHub costOptimizationHub,
        AwsCur cur,
        AwsCustomerProfiles customerProfiles,
        AwsDatabrew databrew,
        AwsDataexchange dataexchange,
        AwsDatapipeline datapipeline,
        AwsDatasync datasync,
        AwsDatazone datazone,
        AwsDax dax,
        AwsDeploy deploy,
        AwsDetective detective,
        AwsDevicefarm devicefarm,
        AwsDevopsGuru devopsGuru,
        AwsDirectconnect directconnect,
        AwsDiscovery discovery,
        AwsDlm dlm,
        AwsDms dms,
        AwsDocdb docdb,
        AwsDocdbElastic docdbElastic,
        AwsDrs drs,
        AwsDs ds,
        AwsDynamodb dynamodb,
        AwsDynamodbstreams dynamodbstreams,
        AwsEbs ebs,
        AwsEc2 ec2,
        AwsEc2InstanceConnect ec2InstanceConnect,
        AwsEcr ecr,
        AwsEcrPublic ecrPublic,
        AwsEcs ecs,
        AwsEfs efs,
        AwsEks eks,
        AwsEksAuth eksAuth,
        AwsElasticInference elasticInference,
        AwsElasticache elasticache,
        AwsElasticbeanstalk elasticbeanstalk,
        AwsElastictranscoder elastictranscoder,
        AwsElb elb,
        AwsElbv2 elbv2,
        AwsEmr emr,
        AwsEmrContainers emrContainers,
        AwsEmrServerless emrServerless,
        AwsEntityresolution entityresolution,
        AwsEs es,
        AwsEvents events,
        AwsEvidently evidently,
        AwsFinspace finspace,
        AwsFirehose firehose,
        AwsFis fis,
        AwsFms fms,
        AwsForecast forecast,
        AwsForecastquery forecastquery,
        AwsFrauddetector frauddetector,
        AwsFreetier freetier,
        AwsFsx fsx,
        AwsGamelift gamelift,
        AwsGlacier glacier,
        AwsGlobalaccelerator globalaccelerator,
        AwsGlue glue,
        AwsGrafana grafana,
        AwsGreengrass greengrass,
        AwsGreengrassv2 greengrassv2,
        AwsGroundstation groundstation,
        AwsGuardduty guardduty,
        AwsHealth health,
        AwsHealthlake healthlake,
        AwsHoneycode honeycode,
        AwsIam iam,
        AwsIdentitystore identitystore,
        AwsImagebuilder imagebuilder,
        AwsImportexport importexport,
        AwsInspector inspector,
        AwsInspectorScan inspectorScan,
        AwsInspector2 inspector2,
        AwsInternetmonitor internetmonitor,
        AwsIot iot,
        AwsIotData iotData,
        AwsIotJobsData iotJobsData,
        AwsIotRoborunner iotRoborunner,
        AwsIot1clickDevices iot1clickDevices,
        AwsIot1clickProjects iot1clickProjects,
        AwsIotanalytics iotanalytics,
        AwsIotdeviceadvisor iotdeviceadvisor,
        AwsIotevents iotevents,
        AwsIoteventsData ioteventsData,
        AwsIotfleethub iotfleethub,
        AwsIotfleetwise iotfleetwise,
        AwsIotsecuretunneling iotsecuretunneling,
        AwsIotsitewise iotsitewise,
        AwsIottwinmaker iottwinmaker,
        AwsIotwireless iotwireless,
        AwsIvs ivs,
        AwsIvsRealtime ivsRealtime,
        AwsIvschat ivschat,
        AwsKafka kafka,
        AwsKafkaconnect kafkaconnect,
        AwsKendra kendra,
        AwsKendraRanking kendraRanking,
        AwsKeyspaces keyspaces,
        AwsKinesis kinesis,
        AwsKinesisVideoArchivedMedia kinesisVideoArchivedMedia,
        AwsKinesisVideoMedia kinesisVideoMedia,
        AwsKinesisVideoSignaling kinesisVideoSignaling,
        AwsKinesisVideoWebrtcStorage kinesisVideoWebrtcStorage,
        AwsKinesisanalytics kinesisanalytics,
        AwsKinesisanalyticsv2 kinesisanalyticsv2,
        AwsKinesisvideo kinesisvideo,
        AwsKms kms,
        AwsLakeformation lakeformation,
        AwsLambda lambda,
        AwsLaunchWizard launchWizard,
        AwsLexModels lexModels,
        AwsLexRuntime lexRuntime,
        AwsLexv2Models lexv2Models,
        AwsLexv2Runtime lexv2Runtime,
        AwsLicenseManager licenseManager,
        AwsLicenseManagerLinuxSubscriptions licenseManagerLinuxSubscriptions,
        AwsLicenseManagerUserSubscriptions licenseManagerUserSubscriptions,
        AwsLightsail lightsail,
        AwsLocation location,
        AwsLogs logs,
        AwsLookoutequipment lookoutequipment,
        AwsLookoutmetrics lookoutmetrics,
        AwsLookoutvision lookoutvision,
        AwsM2 m2,
        AwsMachinelearning machinelearning,
        AwsMacie2 macie2,
        AwsManagedblockchain managedblockchain,
        AwsManagedblockchainQuery managedblockchainQuery,
        AwsMarketplaceAgreement marketplaceAgreement,
        AwsMarketplaceCatalog marketplaceCatalog,
        AwsMarketplaceDeployment marketplaceDeployment,
        AwsMarketplaceEntitlement marketplaceEntitlement,
        AwsMarketplacecommerceanalytics marketplacecommerceanalytics,
        AwsMediaconnect mediaconnect,
        AwsMediaconvert mediaconvert,
        AwsMedialive medialive,
        AwsMediapackage mediapackage,
        AwsMediapackageVod mediapackageVod,
        AwsMediapackagev2 mediapackagev2,
        AwsMediastore mediastore,
        AwsMediastoreData mediastoreData,
        AwsMediatailor mediatailor,
        AwsMedicalImaging medicalImaging,
        AwsMemorydb memorydb,
        AwsMeteringmarketplace meteringmarketplace,
        AwsMgh mgh,
        AwsMgn mgn,
        AwsMigrationHubRefactorSpaces migrationHubRefactorSpaces,
        AwsMigrationhubConfig migrationhubConfig,
        AwsMigrationhuborchestrator migrationhuborchestrator,
        AwsMigrationhubstrategy migrationhubstrategy,
        AwsMobile mobile,
        AwsMq mq,
        AwsMturk mturk,
        AwsMwaa mwaa,
        AwsNeptune neptune,
        AwsNeptuneGraph neptuneGraph,
        AwsNeptunedata neptunedata,
        AwsNetworkFirewall networkFirewall,
        AwsNetworkmanager networkmanager,
        AwsNimble nimble,
        AwsOam oam,
        AwsOmics omics,
        AwsOpensearch opensearch,
        AwsOpensearchserverless opensearchserverless,
        AwsOpsworks opsworks,
        AwsOpsworkscm opsworkscm,
        AwsOrganizations organizations,
        AwsOsis osis,
        AwsOutposts outposts,
        AwsPanorama panorama,
        AwsPaymentCryptography paymentCryptography,
        AwsPaymentCryptographyData paymentCryptographyData,
        AwsPcaConnectorAd pcaConnectorAd,
        AwsPersonalize personalize,
        AwsPersonalizeEvents personalizeEvents,
        AwsPersonalizeRuntime personalizeRuntime,
        AwsPi pi,
        AwsPinpoint pinpoint,
        AwsPinpointEmail pinpointEmail,
        AwsPinpointSmsVoice pinpointSmsVoice,
        AwsPinpointSmsVoiceV2 pinpointSmsVoiceV2,
        AwsPipes pipes,
        AwsPolly polly,
        AwsPricing pricing,
        AwsPrivatenetworks privatenetworks,
        AwsProton proton,
        AwsQbusiness qbusiness,
        AwsQconnect qconnect,
        AwsQldb qldb,
        AwsQldbSession qldbSession,
        AwsQuicksight quicksight,
        AwsRam ram,
        AwsRbin rbin,
        AwsRds rds,
        AwsRdsData rdsData,
        AwsRedshift redshift,
        AwsRedshiftData redshiftData,
        AwsRedshiftServerless redshiftServerless,
        AwsRekognition rekognition,
        AwsRepostspace repostspace,
        AwsResiliencehub resiliencehub,
        AwsResourceExplorer2 resourceExplorer2,
        AwsResourceGroups resourceGroups,
        AwsResourcegroupstaggingapi resourcegroupstaggingapi,
        AwsRobomaker robomaker,
        AwsRolesanywhere rolesanywhere,
        AwsRoute53 route53,
        AwsRoute53RecoveryCluster route53RecoveryCluster,
        AwsRoute53RecoveryControlConfig route53RecoveryControlConfig,
        AwsRoute53RecoveryReadiness route53RecoveryReadiness,
        AwsRoute53domains route53domains,
        AwsRoute53resolver route53resolver,
        AwsRum rum,
        AwsS3api s3api,
        AwsS3control s3control,
        AwsS3outposts s3outposts,
        AwsSagemaker sagemaker,
        AwsSagemakerA2iRuntime sagemakerA2iRuntime,
        AwsSagemakerEdge sagemakerEdge,
        AwsSagemakerFeaturestoreRuntime sagemakerFeaturestoreRuntime,
        AwsSagemakerGeospatial sagemakerGeospatial,
        AwsSagemakerMetrics sagemakerMetrics,
        AwsSagemakerRuntime sagemakerRuntime,
        AwsSavingsplans savingsplans,
        AwsScheduler scheduler,
        AwsSchemas schemas,
        AwsSdb sdb,
        AwsSecretsmanager secretsmanager,
        AwsSecurityhub securityhub,
        AwsSecuritylake securitylake,
        AwsServerlessrepo serverlessrepo,
        AwsServiceQuotas serviceQuotas,
        AwsServicecatalog servicecatalog,
        AwsServicecatalogAppregistry servicecatalogAppregistry,
        AwsServicediscovery servicediscovery,
        AwsSes ses,
        AwsSesv2 sesv2,
        AwsShield shield,
        AwsSigner signer,
        AwsSimspaceweaver simspaceweaver,
        AwsSms sms,
        AwsSnowDeviceManagement snowDeviceManagement,
        AwsSnowball snowball,
        AwsSns sns,
        AwsSqs sqs,
        AwsSsm ssm,
        AwsSsmContacts ssmContacts,
        AwsSsmIncidents ssmIncidents,
        AwsSsmSap ssmSap,
        AwsSso sso,
        AwsSsoAdmin ssoAdmin,
        AwsSsoOidc ssoOidc,
        AwsStepfunctions stepfunctions,
        AwsStoragegateway storagegateway,
        AwsSts sts,
        AwsSupport support,
        AwsSupportApp supportApp,
        AwsSwf swf,
        AwsSynthetics synthetics,
        AwsTextract textract,
        AwsTimestreamQuery timestreamQuery,
        AwsTimestreamWrite timestreamWrite,
        AwsTnb tnb,
        AwsTranscribe transcribe,
        AwsTransfer transfer,
        AwsTranslate translate,
        AwsTrustedadvisor trustedadvisor,
        AwsVerifiedpermissions verifiedpermissions,
        AwsVoiceId voiceId,
        AwsVpcLattice vpcLattice,
        AwsWaf waf,
        AwsWafRegional wafRegional,
        AwsWafv2 wafv2,
        AwsWellarchitected wellarchitected,
        AwsWisdom wisdom,
        AwsWorkdocs workdocs,
        AwsWorkmail workmail,
        AwsWorkmailmessageflow workmailmessageflow,
        AwsWorkspaces workspaces,
        AwsWorkspacesThinClient workspacesThinClient,
        AwsWorkspacesWeb workspacesWeb,
        AwsXray xray,
        ICommand internalCommand
    )
    {
        Accessanalyzer = accessanalyzer;
        Account = account;
        Acm = acm;
        AcmPca = acmPca;
        Alexaforbusiness = alexaforbusiness;
        Amp = amp;
        Amplify = amplify;
        Amplifybackend = amplifybackend;
        Amplifyuibuilder = amplifyuibuilder;
        Apigateway = apigateway;
        Apigatewaymanagementapi = apigatewaymanagementapi;
        Apigatewayv2 = apigatewayv2;
        Appconfig = appconfig;
        Appconfigdata = appconfigdata;
        Appfabric = appfabric;
        Appflow = appflow;
        Appintegrations = appintegrations;
        ApplicationAutoscaling = applicationAutoscaling;
        ApplicationInsights = applicationInsights;
        Applicationcostprofiler = applicationcostprofiler;
        Appmesh = appmesh;
        Apprunner = apprunner;
        Appstream = appstream;
        Appsync = appsync;
        ArcZonalShift = arcZonalShift;
        Athena = athena;
        Auditmanager = auditmanager;
        Autoscaling = autoscaling;
        AutoscalingPlans = autoscalingPlans;
        B2bi = b2bi;
        Backup = backup;
        BackupGateway = backupGateway;
        Backupstorage = backupstorage;
        Batch = batch;
        BcmDataExports = bcmDataExports;
        Bedrock = bedrock;
        BedrockAgent = bedrockAgent;
        BedrockAgentRuntime = bedrockAgentRuntime;
        BedrockRuntime = bedrockRuntime;
        Billingconductor = billingconductor;
        Braket = braket;
        Budgets = budgets;
        Ce = ce;
        Chime = chime;
        ChimeSdkIdentity = chimeSdkIdentity;
        ChimeSdkMediaPipelines = chimeSdkMediaPipelines;
        ChimeSdkMeetings = chimeSdkMeetings;
        ChimeSdkMessaging = chimeSdkMessaging;
        ChimeSdkVoice = chimeSdkVoice;
        Cleanrooms = cleanrooms;
        Cleanroomsml = cleanroomsml;
        Cloud9 = cloud9;
        Cloudcontrol = cloudcontrol;
        Clouddirectory = clouddirectory;
        Cloudformation = cloudformation;
        Cloudfront = cloudfront;
        CloudfrontKeyvaluestore = cloudfrontKeyvaluestore;
        Cloudhsmv2 = cloudhsmv2;
        Cloudsearch = cloudsearch;
        Cloudsearchdomain = cloudsearchdomain;
        Cloudtrail = cloudtrail;
        CloudtrailData = cloudtrailData;
        Cloudwatch = cloudwatch;
        Codeartifact = codeartifact;
        Codebuild = codebuild;
        Codecatalyst = codecatalyst;
        Codecommit = codecommit;
        CodeguruReviewer = codeguruReviewer;
        CodeguruSecurity = codeguruSecurity;
        Codeguruprofiler = codeguruprofiler;
        Codepipeline = codepipeline;
        Codestar = codestar;
        CodestarConnections = codestarConnections;
        CodestarNotifications = codestarNotifications;
        CognitoIdentity = cognitoIdentity;
        CognitoIdp = cognitoIdp;
        CognitoSync = cognitoSync;
        Comprehend = comprehend;
        Comprehendmedical = comprehendmedical;
        ComputeOptimizer = computeOptimizer;
        Configservice = configservice;
        Connect = connect;
        ConnectContactLens = connectContactLens;
        Connectcampaigns = connectcampaigns;
        Connectcases = connectcases;
        Connectparticipant = connectparticipant;
        Controltower = controltower;
        CostOptimizationHub = costOptimizationHub;
        Cur = cur;
        CustomerProfiles = customerProfiles;
        Databrew = databrew;
        Dataexchange = dataexchange;
        Datapipeline = datapipeline;
        Datasync = datasync;
        Datazone = datazone;
        Dax = dax;
        Deploy = deploy;
        Detective = detective;
        Devicefarm = devicefarm;
        DevopsGuru = devopsGuru;
        Directconnect = directconnect;
        Discovery = discovery;
        Dlm = dlm;
        Dms = dms;
        Docdb = docdb;
        DocdbElastic = docdbElastic;
        Drs = drs;
        Ds = ds;
        Dynamodb = dynamodb;
        Dynamodbstreams = dynamodbstreams;
        Ebs = ebs;
        Ec2 = ec2;
        Ec2InstanceConnect = ec2InstanceConnect;
        Ecr = ecr;
        EcrPublic = ecrPublic;
        Ecs = ecs;
        Efs = efs;
        Eks = eks;
        EksAuth = eksAuth;
        ElasticInference = elasticInference;
        Elasticache = elasticache;
        Elasticbeanstalk = elasticbeanstalk;
        Elastictranscoder = elastictranscoder;
        Elb = elb;
        Elbv2 = elbv2;
        Emr = emr;
        EmrContainers = emrContainers;
        EmrServerless = emrServerless;
        Entityresolution = entityresolution;
        Es = es;
        Events = events;
        Evidently = evidently;
        Finspace = finspace;
        Firehose = firehose;
        Fis = fis;
        Fms = fms;
        Forecast = forecast;
        Forecastquery = forecastquery;
        Frauddetector = frauddetector;
        Freetier = freetier;
        Fsx = fsx;
        Gamelift = gamelift;
        Glacier = glacier;
        Globalaccelerator = globalaccelerator;
        Glue = glue;
        Grafana = grafana;
        Greengrass = greengrass;
        Greengrassv2 = greengrassv2;
        Groundstation = groundstation;
        Guardduty = guardduty;
        Health = health;
        Healthlake = healthlake;
        Honeycode = honeycode;
        Iam = iam;
        Identitystore = identitystore;
        Imagebuilder = imagebuilder;
        Importexport = importexport;
        Inspector = inspector;
        InspectorScan = inspectorScan;
        Inspector2 = inspector2;
        Internetmonitor = internetmonitor;
        Iot = iot;
        IotData = iotData;
        IotJobsData = iotJobsData;
        IotRoborunner = iotRoborunner;
        Iot1clickDevices = iot1clickDevices;
        Iot1clickProjects = iot1clickProjects;
        Iotanalytics = iotanalytics;
        Iotdeviceadvisor = iotdeviceadvisor;
        Iotevents = iotevents;
        IoteventsData = ioteventsData;
        Iotfleethub = iotfleethub;
        Iotfleetwise = iotfleetwise;
        Iotsecuretunneling = iotsecuretunneling;
        Iotsitewise = iotsitewise;
        Iottwinmaker = iottwinmaker;
        Iotwireless = iotwireless;
        Ivs = ivs;
        IvsRealtime = ivsRealtime;
        Ivschat = ivschat;
        Kafka = kafka;
        Kafkaconnect = kafkaconnect;
        Kendra = kendra;
        KendraRanking = kendraRanking;
        Keyspaces = keyspaces;
        Kinesis = kinesis;
        KinesisVideoArchivedMedia = kinesisVideoArchivedMedia;
        KinesisVideoMedia = kinesisVideoMedia;
        KinesisVideoSignaling = kinesisVideoSignaling;
        KinesisVideoWebrtcStorage = kinesisVideoWebrtcStorage;
        Kinesisanalytics = kinesisanalytics;
        Kinesisanalyticsv2 = kinesisanalyticsv2;
        Kinesisvideo = kinesisvideo;
        Kms = kms;
        Lakeformation = lakeformation;
        Lambda = lambda;
        LaunchWizard = launchWizard;
        LexModels = lexModels;
        LexRuntime = lexRuntime;
        Lexv2Models = lexv2Models;
        Lexv2Runtime = lexv2Runtime;
        LicenseManager = licenseManager;
        LicenseManagerLinuxSubscriptions = licenseManagerLinuxSubscriptions;
        LicenseManagerUserSubscriptions = licenseManagerUserSubscriptions;
        Lightsail = lightsail;
        Location = location;
        Logs = logs;
        Lookoutequipment = lookoutequipment;
        Lookoutmetrics = lookoutmetrics;
        Lookoutvision = lookoutvision;
        M2 = m2;
        Machinelearning = machinelearning;
        Macie2 = macie2;
        Managedblockchain = managedblockchain;
        ManagedblockchainQuery = managedblockchainQuery;
        MarketplaceAgreement = marketplaceAgreement;
        MarketplaceCatalog = marketplaceCatalog;
        MarketplaceDeployment = marketplaceDeployment;
        MarketplaceEntitlement = marketplaceEntitlement;
        Marketplacecommerceanalytics = marketplacecommerceanalytics;
        Mediaconnect = mediaconnect;
        Mediaconvert = mediaconvert;
        Medialive = medialive;
        Mediapackage = mediapackage;
        MediapackageVod = mediapackageVod;
        Mediapackagev2 = mediapackagev2;
        Mediastore = mediastore;
        MediastoreData = mediastoreData;
        Mediatailor = mediatailor;
        MedicalImaging = medicalImaging;
        Memorydb = memorydb;
        Meteringmarketplace = meteringmarketplace;
        Mgh = mgh;
        Mgn = mgn;
        MigrationHubRefactorSpaces = migrationHubRefactorSpaces;
        MigrationhubConfig = migrationhubConfig;
        Migrationhuborchestrator = migrationhuborchestrator;
        Migrationhubstrategy = migrationhubstrategy;
        Mobile = mobile;
        Mq = mq;
        Mturk = mturk;
        Mwaa = mwaa;
        Neptune = neptune;
        NeptuneGraph = neptuneGraph;
        Neptunedata = neptunedata;
        NetworkFirewall = networkFirewall;
        Networkmanager = networkmanager;
        Nimble = nimble;
        Oam = oam;
        Omics = omics;
        Opensearch = opensearch;
        Opensearchserverless = opensearchserverless;
        Opsworks = opsworks;
        Opsworkscm = opsworkscm;
        Organizations = organizations;
        Osis = osis;
        Outposts = outposts;
        Panorama = panorama;
        PaymentCryptography = paymentCryptography;
        PaymentCryptographyData = paymentCryptographyData;
        PcaConnectorAd = pcaConnectorAd;
        Personalize = personalize;
        PersonalizeEvents = personalizeEvents;
        PersonalizeRuntime = personalizeRuntime;
        Pi = pi;
        Pinpoint = pinpoint;
        PinpointEmail = pinpointEmail;
        PinpointSmsVoice = pinpointSmsVoice;
        PinpointSmsVoiceV2 = pinpointSmsVoiceV2;
        Pipes = pipes;
        Polly = polly;
        Pricing = pricing;
        Privatenetworks = privatenetworks;
        Proton = proton;
        Qbusiness = qbusiness;
        Qconnect = qconnect;
        Qldb = qldb;
        QldbSession = qldbSession;
        Quicksight = quicksight;
        Ram = ram;
        Rbin = rbin;
        Rds = rds;
        RdsData = rdsData;
        Redshift = redshift;
        RedshiftData = redshiftData;
        RedshiftServerless = redshiftServerless;
        Rekognition = rekognition;
        Repostspace = repostspace;
        Resiliencehub = resiliencehub;
        ResourceExplorer2 = resourceExplorer2;
        ResourceGroups = resourceGroups;
        Resourcegroupstaggingapi = resourcegroupstaggingapi;
        Robomaker = robomaker;
        Rolesanywhere = rolesanywhere;
        Route53 = route53;
        Route53RecoveryCluster = route53RecoveryCluster;
        Route53RecoveryControlConfig = route53RecoveryControlConfig;
        Route53RecoveryReadiness = route53RecoveryReadiness;
        Route53domains = route53domains;
        Route53resolver = route53resolver;
        Rum = rum;
        S3api = s3api;
        S3control = s3control;
        S3outposts = s3outposts;
        Sagemaker = sagemaker;
        SagemakerA2iRuntime = sagemakerA2iRuntime;
        SagemakerEdge = sagemakerEdge;
        SagemakerFeaturestoreRuntime = sagemakerFeaturestoreRuntime;
        SagemakerGeospatial = sagemakerGeospatial;
        SagemakerMetrics = sagemakerMetrics;
        SagemakerRuntime = sagemakerRuntime;
        Savingsplans = savingsplans;
        Scheduler = scheduler;
        Schemas = schemas;
        Sdb = sdb;
        Secretsmanager = secretsmanager;
        Securityhub = securityhub;
        Securitylake = securitylake;
        Serverlessrepo = serverlessrepo;
        ServiceQuotas = serviceQuotas;
        Servicecatalog = servicecatalog;
        ServicecatalogAppregistry = servicecatalogAppregistry;
        Servicediscovery = servicediscovery;
        Ses = ses;
        Sesv2 = sesv2;
        Shield = shield;
        Signer = signer;
        Simspaceweaver = simspaceweaver;
        Sms = sms;
        SnowDeviceManagement = snowDeviceManagement;
        Snowball = snowball;
        Sns = sns;
        Sqs = sqs;
        Ssm = ssm;
        SsmContacts = ssmContacts;
        SsmIncidents = ssmIncidents;
        SsmSap = ssmSap;
        Sso = sso;
        SsoAdmin = ssoAdmin;
        SsoOidc = ssoOidc;
        Stepfunctions = stepfunctions;
        Storagegateway = storagegateway;
        Sts = sts;
        Support = support;
        SupportApp = supportApp;
        Swf = swf;
        Synthetics = synthetics;
        Textract = textract;
        TimestreamQuery = timestreamQuery;
        TimestreamWrite = timestreamWrite;
        Tnb = tnb;
        Transcribe = transcribe;
        Transfer = transfer;
        Translate = translate;
        Trustedadvisor = trustedadvisor;
        Verifiedpermissions = verifiedpermissions;
        VoiceId = voiceId;
        VpcLattice = vpcLattice;
        Waf = waf;
        WafRegional = wafRegional;
        Wafv2 = wafv2;
        Wellarchitected = wellarchitected;
        Wisdom = wisdom;
        Workdocs = workdocs;
        Workmail = workmail;
        Workmailmessageflow = workmailmessageflow;
        Workspaces = workspaces;
        WorkspacesThinClient = workspacesThinClient;
        WorkspacesWeb = workspacesWeb;
        Xray = xray;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsAccessanalyzer Accessanalyzer { get; }

    public AwsAccount Account { get; }

    public AwsAcm Acm { get; }

    public AwsAcmPca AcmPca { get; }

    public AwsAlexaforbusiness Alexaforbusiness { get; }

    public AwsAmp Amp { get; }

    public AwsAmplify Amplify { get; }

    public AwsAmplifybackend Amplifybackend { get; }

    public AwsAmplifyuibuilder Amplifyuibuilder { get; }

    public AwsApigateway Apigateway { get; }

    public AwsApigatewaymanagementapi Apigatewaymanagementapi { get; }

    public AwsApigatewayv2 Apigatewayv2 { get; }

    public AwsAppconfig Appconfig { get; }

    public AwsAppconfigdata Appconfigdata { get; }

    public AwsAppfabric Appfabric { get; }

    public AwsAppflow Appflow { get; }

    public AwsAppintegrations Appintegrations { get; }

    public AwsApplicationAutoscaling ApplicationAutoscaling { get; }

    public AwsApplicationInsights ApplicationInsights { get; }

    public AwsApplicationcostprofiler Applicationcostprofiler { get; }

    public AwsAppmesh Appmesh { get; }

    public AwsApprunner Apprunner { get; }

    public AwsAppstream Appstream { get; }

    public AwsAppsync Appsync { get; }

    public AwsArcZonalShift ArcZonalShift { get; }

    public AwsAthena Athena { get; }

    public AwsAuditmanager Auditmanager { get; }

    public AwsAutoscaling Autoscaling { get; }

    public AwsAutoscalingPlans AutoscalingPlans { get; }

    public AwsB2bi B2bi { get; }

    public AwsBackup Backup { get; }

    public AwsBackupGateway BackupGateway { get; }

    public AwsBackupstorage Backupstorage { get; }

    public AwsBatch Batch { get; }

    public AwsBcmDataExports BcmDataExports { get; }

    public AwsBedrock Bedrock { get; }

    public AwsBedrockAgent BedrockAgent { get; }

    public AwsBedrockAgentRuntime BedrockAgentRuntime { get; }

    public AwsBedrockRuntime BedrockRuntime { get; }

    public AwsBillingconductor Billingconductor { get; }

    public AwsBraket Braket { get; }

    public AwsBudgets Budgets { get; }

    public AwsCe Ce { get; }

    public AwsChime Chime { get; }

    public AwsChimeSdkIdentity ChimeSdkIdentity { get; }

    public AwsChimeSdkMediaPipelines ChimeSdkMediaPipelines { get; }

    public AwsChimeSdkMeetings ChimeSdkMeetings { get; }

    public AwsChimeSdkMessaging ChimeSdkMessaging { get; }

    public AwsChimeSdkVoice ChimeSdkVoice { get; }

    public AwsCleanrooms Cleanrooms { get; }

    public AwsCleanroomsml Cleanroomsml { get; }

    public AwsCloud9 Cloud9 { get; }

    public AwsCloudcontrol Cloudcontrol { get; }

    public AwsClouddirectory Clouddirectory { get; }

    public AwsCloudformation Cloudformation { get; }

    public AwsCloudfront Cloudfront { get; }

    public AwsCloudfrontKeyvaluestore CloudfrontKeyvaluestore { get; }

    public AwsCloudhsmv2 Cloudhsmv2 { get; }

    public AwsCloudsearch Cloudsearch { get; }

    public AwsCloudsearchdomain Cloudsearchdomain { get; }

    public AwsCloudtrail Cloudtrail { get; }

    public AwsCloudtrailData CloudtrailData { get; }

    public AwsCloudwatch Cloudwatch { get; }

    public AwsCodeartifact Codeartifact { get; }

    public AwsCodebuild Codebuild { get; }

    public AwsCodecatalyst Codecatalyst { get; }

    public AwsCodecommit Codecommit { get; }

    public AwsCodeguruReviewer CodeguruReviewer { get; }

    public AwsCodeguruSecurity CodeguruSecurity { get; }

    public AwsCodeguruprofiler Codeguruprofiler { get; }

    public AwsCodepipeline Codepipeline { get; }

    public AwsCodestar Codestar { get; }

    public AwsCodestarConnections CodestarConnections { get; }

    public AwsCodestarNotifications CodestarNotifications { get; }

    public AwsCognitoIdentity CognitoIdentity { get; }

    public AwsCognitoIdp CognitoIdp { get; }

    public AwsCognitoSync CognitoSync { get; }

    public AwsComprehend Comprehend { get; }

    public AwsComprehendmedical Comprehendmedical { get; }

    public AwsComputeOptimizer ComputeOptimizer { get; }

    public AwsConfigservice Configservice { get; }

    public AwsConnect Connect { get; }

    public AwsConnectContactLens ConnectContactLens { get; }

    public AwsConnectcampaigns Connectcampaigns { get; }

    public AwsConnectcases Connectcases { get; }

    public AwsConnectparticipant Connectparticipant { get; }

    public AwsControltower Controltower { get; }

    public AwsCostOptimizationHub CostOptimizationHub { get; }

    public AwsCur Cur { get; }

    public AwsCustomerProfiles CustomerProfiles { get; }

    public AwsDatabrew Databrew { get; }

    public AwsDataexchange Dataexchange { get; }

    public AwsDatapipeline Datapipeline { get; }

    public AwsDatasync Datasync { get; }

    public AwsDatazone Datazone { get; }

    public AwsDax Dax { get; }

    public AwsDeploy Deploy { get; }

    public AwsDetective Detective { get; }

    public AwsDevicefarm Devicefarm { get; }

    public AwsDevopsGuru DevopsGuru { get; }

    public AwsDirectconnect Directconnect { get; }

    public AwsDiscovery Discovery { get; }

    public AwsDlm Dlm { get; }

    public AwsDms Dms { get; }

    public AwsDocdb Docdb { get; }

    public AwsDocdbElastic DocdbElastic { get; }

    public AwsDrs Drs { get; }

    public AwsDs Ds { get; }

    public AwsDynamodb Dynamodb { get; }

    public AwsDynamodbstreams Dynamodbstreams { get; }

    public AwsEbs Ebs { get; }

    public AwsEc2 Ec2 { get; }

    public AwsEc2InstanceConnect Ec2InstanceConnect { get; }

    public AwsEcr Ecr { get; }

    public AwsEcrPublic EcrPublic { get; }

    public AwsEcs Ecs { get; }

    public AwsEfs Efs { get; }

    public AwsEks Eks { get; }

    public AwsEksAuth EksAuth { get; }

    public AwsElasticInference ElasticInference { get; }

    public AwsElasticache Elasticache { get; }

    public AwsElasticbeanstalk Elasticbeanstalk { get; }

    public AwsElastictranscoder Elastictranscoder { get; }

    public AwsElb Elb { get; }

    public AwsElbv2 Elbv2 { get; }

    public AwsEmr Emr { get; }

    public AwsEmrContainers EmrContainers { get; }

    public AwsEmrServerless EmrServerless { get; }

    public AwsEntityresolution Entityresolution { get; }

    public AwsEs Es { get; }

    public AwsEvents Events { get; }

    public AwsEvidently Evidently { get; }

    public AwsFinspace Finspace { get; }

    public AwsFirehose Firehose { get; }

    public AwsFis Fis { get; }

    public AwsFms Fms { get; }

    public AwsForecast Forecast { get; }

    public AwsForecastquery Forecastquery { get; }

    public AwsFrauddetector Frauddetector { get; }

    public AwsFreetier Freetier { get; }

    public AwsFsx Fsx { get; }

    public AwsGamelift Gamelift { get; }

    public AwsGlacier Glacier { get; }

    public AwsGlobalaccelerator Globalaccelerator { get; }

    public AwsGlue Glue { get; }

    public AwsGrafana Grafana { get; }

    public AwsGreengrass Greengrass { get; }

    public AwsGreengrassv2 Greengrassv2 { get; }

    public AwsGroundstation Groundstation { get; }

    public AwsGuardduty Guardduty { get; }

    public AwsHealth Health { get; }

    public AwsHealthlake Healthlake { get; }

    public AwsHoneycode Honeycode { get; }

    public AwsIam Iam { get; }

    public AwsIdentitystore Identitystore { get; }

    public AwsImagebuilder Imagebuilder { get; }

    public AwsImportexport Importexport { get; }

    public AwsInspector Inspector { get; }

    public AwsInspectorScan InspectorScan { get; }

    public AwsInspector2 Inspector2 { get; }

    public AwsInternetmonitor Internetmonitor { get; }

    public AwsIot Iot { get; }

    public AwsIotData IotData { get; }

    public AwsIotJobsData IotJobsData { get; }

    public AwsIotRoborunner IotRoborunner { get; }

    public AwsIot1clickDevices Iot1clickDevices { get; }

    public AwsIot1clickProjects Iot1clickProjects { get; }

    public AwsIotanalytics Iotanalytics { get; }

    public AwsIotdeviceadvisor Iotdeviceadvisor { get; }

    public AwsIotevents Iotevents { get; }

    public AwsIoteventsData IoteventsData { get; }

    public AwsIotfleethub Iotfleethub { get; }

    public AwsIotfleetwise Iotfleetwise { get; }

    public AwsIotsecuretunneling Iotsecuretunneling { get; }

    public AwsIotsitewise Iotsitewise { get; }

    public AwsIottwinmaker Iottwinmaker { get; }

    public AwsIotwireless Iotwireless { get; }

    public AwsIvs Ivs { get; }

    public AwsIvsRealtime IvsRealtime { get; }

    public AwsIvschat Ivschat { get; }

    public AwsKafka Kafka { get; }

    public AwsKafkaconnect Kafkaconnect { get; }

    public AwsKendra Kendra { get; }

    public AwsKendraRanking KendraRanking { get; }

    public AwsKeyspaces Keyspaces { get; }

    public AwsKinesis Kinesis { get; }

    public AwsKinesisVideoArchivedMedia KinesisVideoArchivedMedia { get; }

    public AwsKinesisVideoMedia KinesisVideoMedia { get; }

    public AwsKinesisVideoSignaling KinesisVideoSignaling { get; }

    public AwsKinesisVideoWebrtcStorage KinesisVideoWebrtcStorage { get; }

    public AwsKinesisanalytics Kinesisanalytics { get; }

    public AwsKinesisanalyticsv2 Kinesisanalyticsv2 { get; }

    public AwsKinesisvideo Kinesisvideo { get; }

    public AwsKms Kms { get; }

    public AwsLakeformation Lakeformation { get; }

    public AwsLambda Lambda { get; }

    public AwsLaunchWizard LaunchWizard { get; }

    public AwsLexModels LexModels { get; }

    public AwsLexRuntime LexRuntime { get; }

    public AwsLexv2Models Lexv2Models { get; }

    public AwsLexv2Runtime Lexv2Runtime { get; }

    public AwsLicenseManager LicenseManager { get; }

    public AwsLicenseManagerLinuxSubscriptions LicenseManagerLinuxSubscriptions { get; }

    public AwsLicenseManagerUserSubscriptions LicenseManagerUserSubscriptions { get; }

    public AwsLightsail Lightsail { get; }

    public AwsLocation Location { get; }

    public AwsLogs Logs { get; }

    public AwsLookoutequipment Lookoutequipment { get; }

    public AwsLookoutmetrics Lookoutmetrics { get; }

    public AwsLookoutvision Lookoutvision { get; }

    public AwsM2 M2 { get; }

    public AwsMachinelearning Machinelearning { get; }

    public AwsMacie2 Macie2 { get; }

    public AwsManagedblockchain Managedblockchain { get; }

    public AwsManagedblockchainQuery ManagedblockchainQuery { get; }

    public AwsMarketplaceAgreement MarketplaceAgreement { get; }

    public AwsMarketplaceCatalog MarketplaceCatalog { get; }

    public AwsMarketplaceDeployment MarketplaceDeployment { get; }

    public AwsMarketplaceEntitlement MarketplaceEntitlement { get; }

    public AwsMarketplacecommerceanalytics Marketplacecommerceanalytics { get; }

    public AwsMediaconnect Mediaconnect { get; }

    public AwsMediaconvert Mediaconvert { get; }

    public AwsMedialive Medialive { get; }

    public AwsMediapackage Mediapackage { get; }

    public AwsMediapackageVod MediapackageVod { get; }

    public AwsMediapackagev2 Mediapackagev2 { get; }

    public AwsMediastore Mediastore { get; }

    public AwsMediastoreData MediastoreData { get; }

    public AwsMediatailor Mediatailor { get; }

    public AwsMedicalImaging MedicalImaging { get; }

    public AwsMemorydb Memorydb { get; }

    public AwsMeteringmarketplace Meteringmarketplace { get; }

    public AwsMgh Mgh { get; }

    public AwsMgn Mgn { get; }

    public AwsMigrationHubRefactorSpaces MigrationHubRefactorSpaces { get; }

    public AwsMigrationhubConfig MigrationhubConfig { get; }

    public AwsMigrationhuborchestrator Migrationhuborchestrator { get; }

    public AwsMigrationhubstrategy Migrationhubstrategy { get; }

    public AwsMobile Mobile { get; }

    public AwsMq Mq { get; }

    public AwsMturk Mturk { get; }

    public AwsMwaa Mwaa { get; }

    public AwsNeptune Neptune { get; }

    public AwsNeptuneGraph NeptuneGraph { get; }

    public AwsNeptunedata Neptunedata { get; }

    public AwsNetworkFirewall NetworkFirewall { get; }

    public AwsNetworkmanager Networkmanager { get; }

    public AwsNimble Nimble { get; }

    public AwsOam Oam { get; }

    public AwsOmics Omics { get; }

    public AwsOpensearch Opensearch { get; }

    public AwsOpensearchserverless Opensearchserverless { get; }

    public AwsOpsworks Opsworks { get; }

    public AwsOpsworkscm Opsworkscm { get; }

    public AwsOrganizations Organizations { get; }

    public AwsOsis Osis { get; }

    public AwsOutposts Outposts { get; }

    public AwsPanorama Panorama { get; }

    public AwsPaymentCryptography PaymentCryptography { get; }

    public AwsPaymentCryptographyData PaymentCryptographyData { get; }

    public AwsPcaConnectorAd PcaConnectorAd { get; }

    public AwsPersonalize Personalize { get; }

    public AwsPersonalizeEvents PersonalizeEvents { get; }

    public AwsPersonalizeRuntime PersonalizeRuntime { get; }

    public AwsPi Pi { get; }

    public AwsPinpoint Pinpoint { get; }

    public AwsPinpointEmail PinpointEmail { get; }

    public AwsPinpointSmsVoice PinpointSmsVoice { get; }

    public AwsPinpointSmsVoiceV2 PinpointSmsVoiceV2 { get; }

    public AwsPipes Pipes { get; }

    public AwsPolly Polly { get; }

    public AwsPricing Pricing { get; }

    public AwsPrivatenetworks Privatenetworks { get; }

    public AwsProton Proton { get; }

    public AwsQbusiness Qbusiness { get; }

    public AwsQconnect Qconnect { get; }

    public AwsQldb Qldb { get; }

    public AwsQldbSession QldbSession { get; }

    public AwsQuicksight Quicksight { get; }

    public AwsRam Ram { get; }

    public AwsRbin Rbin { get; }

    public AwsRds Rds { get; }

    public AwsRdsData RdsData { get; }

    public AwsRedshift Redshift { get; }

    public AwsRedshiftData RedshiftData { get; }

    public AwsRedshiftServerless RedshiftServerless { get; }

    public AwsRekognition Rekognition { get; }

    public AwsRepostspace Repostspace { get; }

    public AwsResiliencehub Resiliencehub { get; }

    public AwsResourceExplorer2 ResourceExplorer2 { get; }

    public AwsResourceGroups ResourceGroups { get; }

    public AwsResourcegroupstaggingapi Resourcegroupstaggingapi { get; }

    public AwsRobomaker Robomaker { get; }

    public AwsRolesanywhere Rolesanywhere { get; }

    public AwsRoute53 Route53 { get; }

    public AwsRoute53RecoveryCluster Route53RecoveryCluster { get; }

    public AwsRoute53RecoveryControlConfig Route53RecoveryControlConfig { get; }

    public AwsRoute53RecoveryReadiness Route53RecoveryReadiness { get; }

    public AwsRoute53domains Route53domains { get; }

    public AwsRoute53resolver Route53resolver { get; }

    public AwsRum Rum { get; }

    public AwsS3api S3api { get; }

    public AwsS3control S3control { get; }

    public AwsS3outposts S3outposts { get; }

    public AwsSagemaker Sagemaker { get; }

    public AwsSagemakerA2iRuntime SagemakerA2iRuntime { get; }

    public AwsSagemakerEdge SagemakerEdge { get; }

    public AwsSagemakerFeaturestoreRuntime SagemakerFeaturestoreRuntime { get; }

    public AwsSagemakerGeospatial SagemakerGeospatial { get; }

    public AwsSagemakerMetrics SagemakerMetrics { get; }

    public AwsSagemakerRuntime SagemakerRuntime { get; }

    public AwsSavingsplans Savingsplans { get; }

    public AwsScheduler Scheduler { get; }

    public AwsSchemas Schemas { get; }

    public AwsSdb Sdb { get; }

    public AwsSecretsmanager Secretsmanager { get; }

    public AwsSecurityhub Securityhub { get; }

    public AwsSecuritylake Securitylake { get; }

    public AwsServerlessrepo Serverlessrepo { get; }

    public AwsServiceQuotas ServiceQuotas { get; }

    public AwsServicecatalog Servicecatalog { get; }

    public AwsServicecatalogAppregistry ServicecatalogAppregistry { get; }

    public AwsServicediscovery Servicediscovery { get; }

    public AwsSes Ses { get; }

    public AwsSesv2 Sesv2 { get; }

    public AwsShield Shield { get; }

    public AwsSigner Signer { get; }

    public AwsSimspaceweaver Simspaceweaver { get; }

    public AwsSms Sms { get; }

    public AwsSnowDeviceManagement SnowDeviceManagement { get; }

    public AwsSnowball Snowball { get; }

    public AwsSns Sns { get; }

    public AwsSqs Sqs { get; }

    public AwsSsm Ssm { get; }

    public AwsSsmContacts SsmContacts { get; }

    public AwsSsmIncidents SsmIncidents { get; }

    public AwsSsmSap SsmSap { get; }

    public AwsSso Sso { get; }

    public AwsSsoAdmin SsoAdmin { get; }

    public AwsSsoOidc SsoOidc { get; }

    public AwsStepfunctions Stepfunctions { get; }

    public AwsStoragegateway Storagegateway { get; }

    public AwsSts Sts { get; }

    public AwsSupport Support { get; }

    public AwsSupportApp SupportApp { get; }

    public AwsSwf Swf { get; }

    public AwsSynthetics Synthetics { get; }

    public AwsTextract Textract { get; }

    public AwsTimestreamQuery TimestreamQuery { get; }

    public AwsTimestreamWrite TimestreamWrite { get; }

    public AwsTnb Tnb { get; }

    public AwsTranscribe Transcribe { get; }

    public AwsTransfer Transfer { get; }

    public AwsTranslate Translate { get; }

    public AwsTrustedadvisor Trustedadvisor { get; }

    public AwsVerifiedpermissions Verifiedpermissions { get; }

    public AwsVoiceId VoiceId { get; }

    public AwsVpcLattice VpcLattice { get; }

    public AwsWaf Waf { get; }

    public AwsWafRegional WafRegional { get; }

    public AwsWafv2 Wafv2 { get; }

    public AwsWellarchitected Wellarchitected { get; }

    public AwsWisdom Wisdom { get; }

    public AwsWorkdocs Workdocs { get; }

    public AwsWorkmail Workmail { get; }

    public AwsWorkmailmessageflow Workmailmessageflow { get; }

    public AwsWorkspaces Workspaces { get; }

    public AwsWorkspacesThinClient WorkspacesThinClient { get; }

    public AwsWorkspacesWeb WorkspacesWeb { get; }

    public AwsXray Xray { get; }

    public async Task<CommandResult> Configure(AwsConfigureOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsConfigureOptions(), token);
    }

    public async Task<CommandResult> History(AwsHistoryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsHistoryOptions(), token);
    }

    public async Task<CommandResult> S3(AwsS3Options? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsS3Options(), token);
    }
}