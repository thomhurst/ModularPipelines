using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "delete-auto-snapshot")]
public record AwsLightsailDeleteAutoSnapshotOptions(
[property: CliOption("--resource-name")] string ResourceName,
[property: CliOption("--date")] string Date
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}