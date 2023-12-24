using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "list-virtual-interface-test-history")]
public record AwsDirectconnectListVirtualInterfaceTestHistoryOptions : AwsOptions
{
    [CommandSwitch("--test-id")]
    public string? TestId { get; set; }

    [CommandSwitch("--virtual-interface-id")]
    public string? VirtualInterfaceId { get; set; }

    [CommandSwitch("--bgp-peers")]
    public string[]? BgpPeers { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}