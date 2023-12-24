using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "run-instances")]
public record AwsEc2RunInstancesOptions : AwsOptions
{
    [CommandSwitch("--block-device-mappings")]
    public string[]? BlockDeviceMappings { get; set; }

    [CommandSwitch("--image-id")]
    public string? ImageId { get; set; }

    [CommandSwitch("--instance-type")]
    public string? InstanceType { get; set; }

    [CommandSwitch("--ipv6-address-count")]
    public int? Ipv6AddressCount { get; set; }

    [CommandSwitch("--ipv6-addresses")]
    public string[]? Ipv6Addresses { get; set; }

    [CommandSwitch("--kernel-id")]
    public string? KernelId { get; set; }

    [CommandSwitch("--key-name")]
    public string? KeyName { get; set; }

    [CommandSwitch("--monitoring")]
    public string? Monitoring { get; set; }

    [CommandSwitch("--placement")]
    public string? Placement { get; set; }

    [CommandSwitch("--ramdisk-id")]
    public string? RamdiskId { get; set; }

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--security-groups")]
    public string[]? SecurityGroups { get; set; }

    [CommandSwitch("--subnet-id")]
    public string? SubnetId { get; set; }

    [CommandSwitch("--user-data")]
    public string? UserData { get; set; }

    [CommandSwitch("--additional-info")]
    public string? AdditionalInfo { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--iam-instance-profile")]
    public string? IamInstanceProfile { get; set; }

    [CommandSwitch("--instance-initiated-shutdown-behavior")]
    public string? InstanceInitiatedShutdownBehavior { get; set; }

    [CommandSwitch("--network-interfaces")]
    public string[]? NetworkInterfaces { get; set; }

    [CommandSwitch("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }

    [CommandSwitch("--elastic-gpu-specification")]
    public string[]? ElasticGpuSpecification { get; set; }

    [CommandSwitch("--elastic-inference-accelerators")]
    public string[]? ElasticInferenceAccelerators { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--launch-template")]
    public string? LaunchTemplate { get; set; }

    [CommandSwitch("--instance-market-options")]
    public string? InstanceMarketOptions { get; set; }

    [CommandSwitch("--credit-specification")]
    public string? CreditSpecification { get; set; }

    [CommandSwitch("--cpu-options")]
    public string? CpuOptions { get; set; }

    [CommandSwitch("--capacity-reservation-specification")]
    public string? CapacityReservationSpecification { get; set; }

    [CommandSwitch("--hibernation-options")]
    public string? HibernationOptions { get; set; }

    [CommandSwitch("--license-specifications")]
    public string[]? LicenseSpecifications { get; set; }

    [CommandSwitch("--metadata-options")]
    public string? MetadataOptions { get; set; }

    [CommandSwitch("--enclave-options")]
    public string? EnclaveOptions { get; set; }

    [CommandSwitch("--private-dns-name-options")]
    public string? PrivateDnsNameOptions { get; set; }

    [CommandSwitch("--maintenance-options")]
    public string? MaintenanceOptions { get; set; }

    [CommandSwitch("--count")]
    public string? Count { get; set; }

    [CommandSwitch("--secondary-private-ip-addresses")]
    public string? SecondaryPrivateIpAddresses { get; set; }

    [CommandSwitch("--secondary-private-ip-address-count")]
    public string? SecondaryPrivateIpAddressCount { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}