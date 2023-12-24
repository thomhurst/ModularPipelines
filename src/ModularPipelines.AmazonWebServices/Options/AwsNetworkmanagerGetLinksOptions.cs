using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "get-links")]
public record AwsNetworkmanagerGetLinksOptions(
[property: CommandSwitch("--global-network-id")] string GlobalNetworkId
) : AwsOptions
{
    [CommandSwitch("--link-ids")]
    public string[]? LinkIds { get; set; }

    [CommandSwitch("--site-id")]
    public string? SiteId { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--provider")]
    public string? Provider { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}