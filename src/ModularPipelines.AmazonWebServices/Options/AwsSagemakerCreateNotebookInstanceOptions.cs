using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-notebook-instance")]
public record AwsSagemakerCreateNotebookInstanceOptions(
[property: CliOption("--notebook-instance-name")] string NotebookInstanceName,
[property: CliOption("--instance-type")] string InstanceType,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--subnet-id")]
    public string? SubnetId { get; set; }

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--lifecycle-config-name")]
    public string? LifecycleConfigName { get; set; }

    [CliOption("--direct-internet-access")]
    public string? DirectInternetAccess { get; set; }

    [CliOption("--volume-size-in-gb")]
    public int? VolumeSizeInGb { get; set; }

    [CliOption("--accelerator-types")]
    public string[]? AcceleratorTypes { get; set; }

    [CliOption("--default-code-repository")]
    public string? DefaultCodeRepository { get; set; }

    [CliOption("--additional-code-repositories")]
    public string[]? AdditionalCodeRepositories { get; set; }

    [CliOption("--root-access")]
    public string? RootAccess { get; set; }

    [CliOption("--platform-identifier")]
    public string? PlatformIdentifier { get; set; }

    [CliOption("--instance-metadata-service-configuration")]
    public string? InstanceMetadataServiceConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}