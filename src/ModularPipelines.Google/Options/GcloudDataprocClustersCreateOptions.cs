using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "clusters", "create")]
public record GcloudDataprocClustersCreateOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--action-on-failed-primary-workers")]
    public string? ActionOnFailedPrimaryWorkers { get; set; }

    [CliFlag("--no-address")]
    public bool? NoAddress { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--autoscaling-policy")]
    public string? AutoscalingPolicy { get; set; }

    [CliOption("--bucket")]
    public string? Bucket { get; set; }

    [CliFlag("--confidential-compute")]
    public bool? ConfidentialCompute { get; set; }

    [CliOption("--dataproc-metastore")]
    public string? DataprocMetastore { get; set; }

    [CliOption("--driver-pool-accelerator")]
    public string[]? DriverPoolAccelerator { get; set; }

    [CliOption("--driver-pool-boot-disk-size")]
    public string? DriverPoolBootDiskSize { get; set; }

    [CliOption("--driver-pool-boot-disk-type")]
    public string? DriverPoolBootDiskType { get; set; }

    [CliOption("--driver-pool-id")]
    public string? DriverPoolId { get; set; }

    [CliOption("--driver-pool-local-ssd-interface")]
    public string? DriverPoolLocalSsdInterface { get; set; }

    [CliOption("--driver-pool-machine-type")]
    public string? DriverPoolMachineType { get; set; }

    [CliOption("--driver-pool-min-cpu-platform")]
    public string? DriverPoolMinCpuPlatform { get; set; }

    [CliOption("--driver-pool-size")]
    public string? DriverPoolSize { get; set; }

    [CliFlag("--enable-component-gateway")]
    public bool? EnableComponentGateway { get; set; }

    [CliOption("--initialization-action-timeout")]
    public string? InitializationActionTimeout { get; set; }

    [CliOption("--initialization-actions")]
    public string[]? InitializationActions { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--master-accelerator")]
    public string[]? MasterAccelerator { get; set; }

    [CliOption("--master-boot-disk-size")]
    public string? MasterBootDiskSize { get; set; }

    [CliOption("--master-boot-disk-type")]
    public string? MasterBootDiskType { get; set; }

    [CliOption("--master-local-ssd-interface")]
    public string? MasterLocalSsdInterface { get; set; }

    [CliOption("--master-machine-type")]
    public string? MasterMachineType { get; set; }

    [CliOption("--master-min-cpu-platform")]
    public string? MasterMinCpuPlatform { get; set; }

    [CliOption("--max-idle")]
    public string? MaxIdle { get; set; }

    [CliOption("--min-secondary-worker-fraction")]
    public string? MinSecondaryWorkerFraction { get; set; }

    [CliOption("--node-group")]
    public string? NodeGroup { get; set; }

    [CliOption("--num-driver-pool-local-ssds")]
    public string? NumDriverPoolLocalSsds { get; set; }

    [CliOption("--num-master-local-ssds")]
    public string? NumMasterLocalSsds { get; set; }

    [CliOption("--num-masters")]
    public string? NumMasters { get; set; }

    [CliOption("--num-secondary-worker-local-ssds")]
    public string? NumSecondaryWorkerLocalSsds { get; set; }

    [CliOption("--num-worker-local-ssds")]
    public string? NumWorkerLocalSsds { get; set; }

    [CliOption("--optional-components")]
    public string[]? OptionalComponents { get; set; }

    [CliOption("--private-ipv6-google-access-type")]
    public string? PrivateIpv6GoogleAccessType { get; set; }

    [CliOption("--properties")]
    public string[]? Properties { get; set; }

    [CliOption("--secondary-worker-accelerator")]
    public string[]? SecondaryWorkerAccelerator { get; set; }

    [CliOption("--secondary-worker-boot-disk-size")]
    public string? SecondaryWorkerBootDiskSize { get; set; }

    [CliOption("--secondary-worker-boot-disk-type")]
    public string? SecondaryWorkerBootDiskType { get; set; }

    [CliOption("--secondary-worker-local-ssd-interface")]
    public string? SecondaryWorkerLocalSsdInterface { get; set; }

    [CliOption("--secondary-worker-machine-types")]
    public string[]? SecondaryWorkerMachineTypes { get; set; }

    [CliFlag("--shielded-integrity-monitoring")]
    public bool? ShieldedIntegrityMonitoring { get; set; }

    [CliFlag("--shielded-secure-boot")]
    public bool? ShieldedSecureBoot { get; set; }

    [CliFlag("--shielded-vtpm")]
    public bool? ShieldedVtpm { get; set; }

    [CliOption("--temp-bucket")]
    public string? TempBucket { get; set; }

    [CliOption("--worker-accelerator")]
    public string[]? WorkerAccelerator { get; set; }

    [CliOption("--worker-boot-disk-size")]
    public string? WorkerBootDiskSize { get; set; }

    [CliOption("--worker-boot-disk-type")]
    public string? WorkerBootDiskType { get; set; }

    [CliOption("--worker-local-ssd-interface")]
    public string? WorkerLocalSsdInterface { get; set; }

    [CliOption("--worker-machine-type")]
    public string? WorkerMachineType { get; set; }

    [CliOption("--worker-min-cpu-platform")]
    public string? WorkerMinCpuPlatform { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliOption("--expiration-time")]
    public string? ExpirationTime { get; set; }

    [CliOption("--max-age")]
    public string? MaxAge { get; set; }

    [CliOption("--gce-pd-kms-key")]
    public string? GcePdKmsKey { get; set; }

    [CliOption("--gce-pd-kms-key-keyring")]
    public string? GcePdKmsKeyKeyring { get; set; }

    [CliOption("--gce-pd-kms-key-location")]
    public string? GcePdKmsKeyLocation { get; set; }

    [CliOption("--gce-pd-kms-key-project")]
    public string? GcePdKmsKeyProject { get; set; }

    [CliOption("--identity-config-file")]
    public string? IdentityConfigFile { get; set; }

    [CliOption("--secure-multi-tenancy-user-mapping")]
    public string? SecureMultiTenancyUserMapping { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--image-version")]
    public string? ImageVersion { get; set; }

    [CliOption("--kerberos-config-file")]
    public string? KerberosConfigFile { get; set; }

    [CliFlag("--enable-kerberos")]
    public bool? EnableKerberos { get; set; }

    [CliOption("--kerberos-root-principal-password-uri")]
    public string? KerberosRootPrincipalPasswordUri { get; set; }

    [CliOption("--kerberos-kms-key")]
    public string? KerberosKmsKey { get; set; }

    [CliOption("--kerberos-kms-key-keyring")]
    public string? KerberosKmsKeyKeyring { get; set; }

    [CliOption("--kerberos-kms-key-location")]
    public string? KerberosKmsKeyLocation { get; set; }

    [CliOption("--kerberos-kms-key-project")]
    public string? KerberosKmsKeyProject { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CliOption("--kms-location")]
    public string? KmsLocation { get; set; }

    [CliOption("--kms-project")]
    public string? KmsProject { get; set; }

    [CliOption("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CliOption("--scopes")]
    public string[]? Scopes { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--reservation")]
    public string? Reservation { get; set; }

    [CliOption("--reservation-affinity")]
    public string? ReservationAffinity { get; set; }

    [CliOption("--metric-sources")]
    public string[]? MetricSources { get; set; }

    [CliOption("--metric-overrides")]
    public string[]? MetricOverrides { get; set; }

    [CliOption("--metric-overrides-file")]
    public string? MetricOverridesFile { get; set; }

    [CliFlag("--single-node")]
    public bool? SingleNode { get; set; }

    [CliOption("--min-num-workers")]
    public string? MinNumWorkers { get; set; }

    [CliOption("--num-secondary-workers")]
    public string? NumSecondaryWorkers { get; set; }

    [CliOption("--num-workers")]
    public string? NumWorkers { get; set; }

    [CliOption("--secondary-worker-type")]
    public string? SecondaryWorkerType { get; set; }
}