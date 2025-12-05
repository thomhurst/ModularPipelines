using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-notebook-instance")]
public record AwsSagemakerUpdateNotebookInstanceOptions(
[property: CliOption("--notebook-instance-name")] string NotebookInstanceName
) : AwsOptions
{
    [CliOption("--instance-type")]
    public string? InstanceType { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--lifecycle-config-name")]
    public string? LifecycleConfigName { get; set; }

    [CliOption("--volume-size-in-gb")]
    public int? VolumeSizeInGb { get; set; }

    [CliOption("--default-code-repository")]
    public string? DefaultCodeRepository { get; set; }

    [CliOption("--additional-code-repositories")]
    public string[]? AdditionalCodeRepositories { get; set; }

    [CliOption("--accelerator-types")]
    public string[]? AcceleratorTypes { get; set; }

    [CliOption("--root-access")]
    public string? RootAccess { get; set; }

    [CliOption("--instance-metadata-service-configuration")]
    public string? InstanceMetadataServiceConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}