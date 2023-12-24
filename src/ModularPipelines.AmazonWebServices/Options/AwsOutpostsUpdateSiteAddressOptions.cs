using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("outposts", "update-site-address")]
public record AwsOutpostsUpdateSiteAddressOptions(
[property: CommandSwitch("--site-id")] string SiteId,
[property: CommandSwitch("--address-type")] string AddressType,
[property: CommandSwitch("--address")] string Address
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}