using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "list-virtual-interface-test-history")]
public record AwsDirectconnectListVirtualInterfaceTestHistoryOptions : AwsOptions
{
    [CliOption("--test-id")]
    public string? TestId { get; set; }

    [CliOption("--virtual-interface-id")]
    public string? VirtualInterfaceId { get; set; }

    [CliOption("--bgp-peers")]
    public string[]? BgpPeers { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}