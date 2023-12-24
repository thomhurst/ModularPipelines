using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-notebook-instance")]
public record AwsSagemakerCreateNotebookInstanceOptions(
[property: CommandSwitch("--notebook-instance-name")] string NotebookInstanceName,
[property: CommandSwitch("--instance-type")] string InstanceType,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--subnet-id")]
    public string? SubnetId { get; set; }

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--lifecycle-config-name")]
    public string? LifecycleConfigName { get; set; }

    [CommandSwitch("--direct-internet-access")]
    public string? DirectInternetAccess { get; set; }

    [CommandSwitch("--volume-size-in-gb")]
    public int? VolumeSizeInGb { get; set; }

    [CommandSwitch("--accelerator-types")]
    public string[]? AcceleratorTypes { get; set; }

    [CommandSwitch("--default-code-repository")]
    public string? DefaultCodeRepository { get; set; }

    [CommandSwitch("--additional-code-repositories")]
    public string[]? AdditionalCodeRepositories { get; set; }

    [CommandSwitch("--root-access")]
    public string? RootAccess { get; set; }

    [CommandSwitch("--platform-identifier")]
    public string? PlatformIdentifier { get; set; }

    [CommandSwitch("--instance-metadata-service-configuration")]
    public string? InstanceMetadataServiceConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}