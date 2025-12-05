using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "list-supported-instance-types")]
public record AwsEmrListSupportedInstanceTypesOptions(
[property: CliOption("--release-label")] string ReleaseLabel
) : AwsOptions
{
    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}