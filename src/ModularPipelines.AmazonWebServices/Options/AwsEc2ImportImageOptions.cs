using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "import-image")]
public record AwsEc2ImportImageOptions : AwsOptions
{
    [CliOption("--architecture")]
    public string? Architecture { get; set; }

    [CliOption("--client-data")]
    public string? ClientData { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--disk-containers")]
    public string[]? DiskContainers { get; set; }

    [CliOption("--hypervisor")]
    public string? Hypervisor { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--license-type")]
    public string? LicenseType { get; set; }

    [CliOption("--platform")]
    public string? Platform { get; set; }

    [CliOption("--role-name")]
    public string? RoleName { get; set; }

    [CliOption("--license-specifications")]
    public string[]? LicenseSpecifications { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--usage-operation")]
    public string? UsageOperation { get; set; }

    [CliOption("--boot-mode")]
    public string? BootMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}