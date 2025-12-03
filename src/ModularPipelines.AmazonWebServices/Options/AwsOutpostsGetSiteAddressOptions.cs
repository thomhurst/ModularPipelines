using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("outposts", "get-site-address")]
public record AwsOutpostsGetSiteAddressOptions(
[property: CliOption("--site-id")] string SiteId,
[property: CliOption("--address-type")] string AddressType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}