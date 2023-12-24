using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("outposts", "get-site-address")]
public record AwsOutpostsGetSiteAddressOptions(
[property: CommandSwitch("--site-id")] string SiteId,
[property: CommandSwitch("--address-type")] string AddressType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}