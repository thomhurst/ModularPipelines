using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf", "delete-geo-match-set")]
public record AwsWafDeleteGeoMatchSetOptions(
[property: CliOption("--geo-match-set-id")] string GeoMatchSetId,
[property: CliOption("--change-token")] string ChangeToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}