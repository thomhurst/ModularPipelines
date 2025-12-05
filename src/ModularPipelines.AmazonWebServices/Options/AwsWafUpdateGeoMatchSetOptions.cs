using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf", "update-geo-match-set")]
public record AwsWafUpdateGeoMatchSetOptions(
[property: CliOption("--geo-match-set-id")] string GeoMatchSetId,
[property: CliOption("--change-token")] string ChangeToken,
[property: CliOption("--updates")] string[] Updates
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}