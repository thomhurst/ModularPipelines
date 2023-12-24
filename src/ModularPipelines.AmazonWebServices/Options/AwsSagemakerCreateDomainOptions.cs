using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-domain")]
public record AwsSagemakerCreateDomainOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--auth-mode")] string AuthMode,
[property: CommandSwitch("--default-user-settings")] string DefaultUserSettings,
[property: CommandSwitch("--subnet-ids")] string[] SubnetIds,
[property: CommandSwitch("--vpc-id")] string VpcId
) : AwsOptions
{
    [CommandSwitch("--domain-settings")]
    public string? DomainSettings { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--app-network-access-type")]
    public string? AppNetworkAccessType { get; set; }

    [CommandSwitch("--home-efs-file-system-kms-key-id")]
    public string? HomeEfsFileSystemKmsKeyId { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--app-security-group-management")]
    public string? AppSecurityGroupManagement { get; set; }

    [CommandSwitch("--default-space-settings")]
    public string? DefaultSpaceSettings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}