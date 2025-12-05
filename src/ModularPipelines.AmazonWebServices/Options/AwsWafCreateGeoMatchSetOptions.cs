using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("waf", "create-geo-match-set")]
public record AwsWafCreateGeoMatchSetOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--change-token")] string ChangeToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}