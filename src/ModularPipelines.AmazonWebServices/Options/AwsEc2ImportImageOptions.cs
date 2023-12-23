using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "import-image")]
public record AwsEc2ImportImageOptions : AwsOptions
{
    [CommandSwitch("--architecture")]
    public string? Architecture { get; set; }

    [CommandSwitch("--client-data")]
    public string? ClientData { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--disk-containers")]
    public string[]? DiskContainers { get; set; }

    [CommandSwitch("--hypervisor")]
    public string? Hypervisor { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--license-type")]
    public string? LicenseType { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [CommandSwitch("--role-name")]
    public string? RoleName { get; set; }

    [CommandSwitch("--license-specifications")]
    public string[]? LicenseSpecifications { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--usage-operation")]
    public string? UsageOperation { get; set; }

    [CommandSwitch("--boot-mode")]
    public string? BootMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}