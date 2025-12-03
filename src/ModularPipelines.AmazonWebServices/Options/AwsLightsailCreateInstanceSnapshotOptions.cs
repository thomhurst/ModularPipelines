using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "create-instance-snapshot")]
public record AwsLightsailCreateInstanceSnapshotOptions(
[property: CliOption("--instance-snapshot-name")] string InstanceSnapshotName,
[property: CliOption("--instance-name")] string InstanceName
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}