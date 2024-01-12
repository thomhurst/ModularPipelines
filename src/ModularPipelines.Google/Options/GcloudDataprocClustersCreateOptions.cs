using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "clusters", "create")]
public record GcloudDataprocClustersCreateOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--action-on-failed-primary-workers")]
    public string? ActionOnFailedPrimaryWorkers { get; set; }

    [BooleanCommandSwitch("--no-address")]
    public bool? NoAddress { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--autoscaling-policy")]
    public string? AutoscalingPolicy { get; set; }

    [CommandSwitch("--bucket")]
    public string? Bucket { get; set; }

    [BooleanCommandSwitch("--confidential-compute")]
    public bool? ConfidentialCompute { get; set; }

    [CommandSwitch("--dataproc-metastore")]
    public string? DataprocMetastore { get; set; }

    [CommandSwitch("--driver-pool-accelerator")]
    public string[]? DriverPoolAccelerator { get; set; }

    [CommandSwitch("--driver-pool-boot-disk-size")]
    public string? DriverPoolBootDiskSize { get; set; }

    [CommandSwitch("--driver-pool-boot-disk-type")]
    public string? DriverPoolBootDiskType { get; set; }

    [CommandSwitch("--driver-pool-id")]
    public string? DriverPoolId { get; set; }

    [CommandSwitch("--driver-pool-local-ssd-interface")]
    public string? DriverPoolLocalSsdInterface { get; set; }

    [CommandSwitch("--driver-pool-machine-type")]
    public string? DriverPoolMachineType { get; set; }

    [CommandSwitch("--driver-pool-min-cpu-platform")]
    public string? DriverPoolMinCpuPlatform { get; set; }

    [CommandSwitch("--driver-pool-size")]
    public string? DriverPoolSize { get; set; }

    [BooleanCommandSwitch("--enable-component-gateway")]
    public bool? EnableComponentGateway { get; set; }

    [CommandSwitch("--initialization-action-timeout")]
    public string? InitializationActionTimeout { get; set; }

    [CommandSwitch("--initialization-actions")]
    public string[]? InitializationActions { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--master-accelerator")]
    public string[]? MasterAccelerator { get; set; }

    [CommandSwitch("--master-boot-disk-size")]
    public string? MasterBootDiskSize { get; set; }

    [CommandSwitch("--master-boot-disk-type")]
    public string? MasterBootDiskType { get; set; }

    [CommandSwitch("--master-local-ssd-interface")]
    public string? MasterLocalSsdInterface { get; set; }

    [CommandSwitch("--master-machine-type")]
    public string? MasterMachineType { get; set; }

    [CommandSwitch("--master-min-cpu-platform")]
    public string? MasterMinCpuPlatform { get; set; }

    [CommandSwitch("--max-idle")]
    public string? MaxIdle { get; set; }

    [CommandSwitch("--min-secondary-worker-fraction")]
    public string? MinSecondaryWorkerFraction { get; set; }

    [CommandSwitch("--node-group")]
    public string? NodeGroup { get; set; }

    [CommandSwitch("--num-driver-pool-local-ssds")]
    public string? NumDriverPoolLocalSsds { get; set; }

    [CommandSwitch("--num-master-local-ssds")]
    public string? NumMasterLocalSsds { get; set; }

    [CommandSwitch("--num-masters")]
    public string? NumMasters { get; set; }

    [CommandSwitch("--num-secondary-worker-local-ssds")]
    public string? NumSecondaryWorkerLocalSsds { get; set; }

    [CommandSwitch("--num-worker-local-ssds")]
    public string? NumWorkerLocalSsds { get; set; }

    [CommandSwitch("--optional-components")]
    public string[]? OptionalComponents { get; set; }

    [CommandSwitch("--private-ipv6-google-access-type")]
    public string? PrivateIpv6GoogleAccessType { get; set; }

    [CommandSwitch("--properties")]
    public string[]? Properties { get; set; }

    [CommandSwitch("--secondary-worker-accelerator")]
    public string[]? SecondaryWorkerAccelerator { get; set; }

    [CommandSwitch("--secondary-worker-boot-disk-size")]
    public string? SecondaryWorkerBootDiskSize { get; set; }

    [CommandSwitch("--secondary-worker-boot-disk-type")]
    public string? SecondaryWorkerBootDiskType { get; set; }

    [CommandSwitch("--secondary-worker-local-ssd-interface")]
    public string? SecondaryWorkerLocalSsdInterface { get; set; }

    [CommandSwitch("--secondary-worker-machine-types")]
    public string[]? SecondaryWorkerMachineTypes { get; set; }

    [BooleanCommandSwitch("--shielded-integrity-monitoring")]
    public bool? ShieldedIntegrityMonitoring { get; set; }

    [BooleanCommandSwitch("--shielded-secure-boot")]
    public bool? ShieldedSecureBoot { get; set; }

    [BooleanCommandSwitch("--shielded-vtpm")]
    public bool? ShieldedVtpm { get; set; }

    [CommandSwitch("--temp-bucket")]
    public string? TempBucket { get; set; }

    [CommandSwitch("--worker-accelerator")]
    public string[]? WorkerAccelerator { get; set; }

    [CommandSwitch("--worker-boot-disk-size")]
    public string? WorkerBootDiskSize { get; set; }

    [CommandSwitch("--worker-boot-disk-type")]
    public string? WorkerBootDiskType { get; set; }

    [CommandSwitch("--worker-local-ssd-interface")]
    public string? WorkerLocalSsdInterface { get; set; }

    [CommandSwitch("--worker-machine-type")]
    public string? WorkerMachineType { get; set; }

    [CommandSwitch("--worker-min-cpu-platform")]
    public string? WorkerMinCpuPlatform { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [CommandSwitch("--expiration-time")]
    public string? ExpirationTime { get; set; }

    [CommandSwitch("--max-age")]
    public string? MaxAge { get; set; }

    [CommandSwitch("--gce-pd-kms-key")]
    public string? GcePdKmsKey { get; set; }

    [CommandSwitch("--gce-pd-kms-key-keyring")]
    public string? GcePdKmsKeyKeyring { get; set; }

    [CommandSwitch("--gce-pd-kms-key-location")]
    public string? GcePdKmsKeyLocation { get; set; }

    [CommandSwitch("--gce-pd-kms-key-project")]
    public string? GcePdKmsKeyProject { get; set; }

    [CommandSwitch("--identity-config-file")]
    public string? IdentityConfigFile { get; set; }

    [CommandSwitch("--secure-multi-tenancy-user-mapping")]
    public string? SecureMultiTenancyUserMapping { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--image-version")]
    public string? ImageVersion { get; set; }

    [CommandSwitch("--kerberos-config-file")]
    public string? KerberosConfigFile { get; set; }

    [BooleanCommandSwitch("--enable-kerberos")]
    public bool? EnableKerberos { get; set; }

    [CommandSwitch("--kerberos-root-principal-password-uri")]
    public string? KerberosRootPrincipalPasswordUri { get; set; }

    [CommandSwitch("--kerberos-kms-key")]
    public string? KerberosKmsKey { get; set; }

    [CommandSwitch("--kerberos-kms-key-keyring")]
    public string? KerberosKmsKeyKeyring { get; set; }

    [CommandSwitch("--kerberos-kms-key-location")]
    public string? KerberosKmsKeyLocation { get; set; }

    [CommandSwitch("--kerberos-kms-key-project")]
    public string? KerberosKmsKeyProject { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CommandSwitch("--kms-location")]
    public string? KmsLocation { get; set; }

    [CommandSwitch("--kms-project")]
    public string? KmsProject { get; set; }

    [CommandSwitch("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CommandSwitch("--scopes")]
    public string[]? Scopes { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--reservation")]
    public string? Reservation { get; set; }

    [CommandSwitch("--reservation-affinity")]
    public string? ReservationAffinity { get; set; }

    [CommandSwitch("--metric-sources")]
    public string[]? MetricSources { get; set; }

    [CommandSwitch("--metric-overrides")]
    public string[]? MetricOverrides { get; set; }

    [CommandSwitch("--metric-overrides-file")]
    public string? MetricOverridesFile { get; set; }

    [BooleanCommandSwitch("--single-node")]
    public bool? SingleNode { get; set; }

    [CommandSwitch("--min-num-workers")]
    public string? MinNumWorkers { get; set; }

    [CommandSwitch("--num-secondary-workers")]
    public string? NumSecondaryWorkers { get; set; }

    [CommandSwitch("--num-workers")]
    public string? NumWorkers { get; set; }

    [CommandSwitch("--secondary-worker-type")]
    public string? SecondaryWorkerType { get; set; }
}