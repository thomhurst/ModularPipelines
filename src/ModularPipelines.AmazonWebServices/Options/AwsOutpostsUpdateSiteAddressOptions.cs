using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("outposts", "update-site-address")]
public record AwsOutpostsUpdateSiteAddressOptions(
[property: CliOption("--site-id")] string SiteId,
[property: CliOption("--address-type")] string AddressType,
[property: CliOption("--address")] string Address
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}