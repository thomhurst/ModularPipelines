using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-notebook-instance")]
public record AwsSagemakerUpdateNotebookInstanceOptions(
[property: CommandSwitch("--notebook-instance-name")] string NotebookInstanceName
) : AwsOptions
{
    [CommandSwitch("--instance-type")]
    public string? InstanceType { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--lifecycle-config-name")]
    public string? LifecycleConfigName { get; set; }

    [CommandSwitch("--volume-size-in-gb")]
    public int? VolumeSizeInGb { get; set; }

    [CommandSwitch("--default-code-repository")]
    public string? DefaultCodeRepository { get; set; }

    [CommandSwitch("--additional-code-repositories")]
    public string[]? AdditionalCodeRepositories { get; set; }

    [CommandSwitch("--accelerator-types")]
    public string[]? AcceleratorTypes { get; set; }

    [CommandSwitch("--root-access")]
    public string? RootAccess { get; set; }

    [CommandSwitch("--instance-metadata-service-configuration")]
    public string? InstanceMetadataServiceConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}