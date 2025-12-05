using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "update-link")]
public record AwsNetworkmanagerUpdateLinkOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--link-id")] string LinkId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--bandwidth")]
    public string? Bandwidth { get; set; }

    [CliOption("--provider")]
    public string? Provider { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}