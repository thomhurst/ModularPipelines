using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "stop-bgp-failover-test")]
public record AwsDirectconnectStopBgpFailoverTestOptions(
[property: CliOption("--virtual-interface-id")] string VirtualInterfaceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}