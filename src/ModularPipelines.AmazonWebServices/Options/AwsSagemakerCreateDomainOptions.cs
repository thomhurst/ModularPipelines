using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-domain")]
public record AwsSagemakerCreateDomainOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--auth-mode")] string AuthMode,
[property: CliOption("--default-user-settings")] string DefaultUserSettings,
[property: CliOption("--subnet-ids")] string[] SubnetIds,
[property: CliOption("--vpc-id")] string VpcId
) : AwsOptions
{
    [CliOption("--domain-settings")]
    public string? DomainSettings { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--app-network-access-type")]
    public string? AppNetworkAccessType { get; set; }

    [CliOption("--home-efs-file-system-kms-key-id")]
    public string? HomeEfsFileSystemKmsKeyId { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--app-security-group-management")]
    public string? AppSecurityGroupManagement { get; set; }

    [CliOption("--default-space-settings")]
    public string? DefaultSpaceSettings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}