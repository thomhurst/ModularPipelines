using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "create-link")]
public record AwsNetworkmanagerCreateLinkOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--bandwidth")] string Bandwidth,
[property: CliOption("--site-id")] string SiteId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--provider")]
    public string? Provider { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}