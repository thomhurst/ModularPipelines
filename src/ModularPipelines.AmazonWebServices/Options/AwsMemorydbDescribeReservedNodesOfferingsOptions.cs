using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("memorydb", "describe-reserved-nodes-offerings")]
public record AwsMemorydbDescribeReservedNodesOfferingsOptions : AwsOptions
{
    [CommandSwitch("--reserved-nodes-offering-id")]
    public string? ReservedNodesOfferingId { get; set; }

    [CommandSwitch("--node-type")]
    public string? NodeType { get; set; }

    [CommandSwitch("--duration")]
    public string? Duration { get; set; }

    [CommandSwitch("--offering-type")]
    public string? OfferingType { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}