using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "attach-disk")]
public record AwsLightsailAttachDiskOptions(
[property: CliOption("--disk-name")] string DiskName,
[property: CliOption("--instance-name")] string InstanceName,
[property: CliOption("--disk-path")] string DiskPath
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}