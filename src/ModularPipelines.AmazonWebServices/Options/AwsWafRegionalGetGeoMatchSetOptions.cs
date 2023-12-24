using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("waf-regional", "get-geo-match-set")]
public record AwsWafRegionalGetGeoMatchSetOptions(
[property: CommandSwitch("--geo-match-set-id")] string GeoMatchSetId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}