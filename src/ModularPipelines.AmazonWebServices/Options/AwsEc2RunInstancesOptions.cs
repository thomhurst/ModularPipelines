using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "run-instances")]
public record AwsEc2RunInstancesOptions : AwsOptions
{
    [CliOption("--block-device-mappings")]
    public string[]? BlockDeviceMappings { get; set; }

    [CliOption("--image-id")]
    public string? ImageId { get; set; }

    [CliOption("--instance-type")]
    public string? InstanceType { get; set; }

    [CliOption("--ipv6-address-count")]
    public int? Ipv6AddressCount { get; set; }

    [CliOption("--ipv6-addresses")]
    public string[]? Ipv6Addresses { get; set; }

    [CliOption("--kernel-id")]
    public string? KernelId { get; set; }

    [CliOption("--key-name")]
    public string? KeyName { get; set; }

    [CliOption("--monitoring")]
    public string? Monitoring { get; set; }

    [CliOption("--placement")]
    public string? Placement { get; set; }

    [CliOption("--ramdisk-id")]
    public string? RamdiskId { get; set; }

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--security-groups")]
    public string[]? SecurityGroups { get; set; }

    [CliOption("--subnet-id")]
    public string? SubnetId { get; set; }

    [CliOption("--user-data")]
    public string? UserData { get; set; }

    [CliOption("--additional-info")]
    public string? AdditionalInfo { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--iam-instance-profile")]
    public string? IamInstanceProfile { get; set; }

    [CliOption("--instance-initiated-shutdown-behavior")]
    public string? InstanceInitiatedShutdownBehavior { get; set; }

    [CliOption("--network-interfaces")]
    public string[]? NetworkInterfaces { get; set; }

    [CliOption("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }

    [CliOption("--elastic-gpu-specification")]
    public string[]? ElasticGpuSpecification { get; set; }

    [CliOption("--elastic-inference-accelerators")]
    public string[]? ElasticInferenceAccelerators { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--launch-template")]
    public string? LaunchTemplate { get; set; }

    [CliOption("--instance-market-options")]
    public string? InstanceMarketOptions { get; set; }

    [CliOption("--credit-specification")]
    public string? CreditSpecification { get; set; }

    [CliOption("--cpu-options")]
    public string? CpuOptions { get; set; }

    [CliOption("--capacity-reservation-specification")]
    public string? CapacityReservationSpecification { get; set; }

    [CliOption("--hibernation-options")]
    public string? HibernationOptions { get; set; }

    [CliOption("--license-specifications")]
    public string[]? LicenseSpecifications { get; set; }

    [CliOption("--metadata-options")]
    public string? MetadataOptions { get; set; }

    [CliOption("--enclave-options")]
    public string? EnclaveOptions { get; set; }

    [CliOption("--private-dns-name-options")]
    public string? PrivateDnsNameOptions { get; set; }

    [CliOption("--maintenance-options")]
    public string? MaintenanceOptions { get; set; }

    [CliOption("--count")]
    public string? Count { get; set; }

    [CliOption("--secondary-private-ip-addresses")]
    public string? SecondaryPrivateIpAddresses { get; set; }

    [CliOption("--secondary-private-ip-address-count")]
    public string? SecondaryPrivateIpAddressCount { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}