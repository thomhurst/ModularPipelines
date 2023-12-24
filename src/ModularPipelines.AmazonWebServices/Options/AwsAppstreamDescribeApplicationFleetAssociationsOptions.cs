using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "describe-application-fleet-associations")]
public record AwsAppstreamDescribeApplicationFleetAssociationsOptions : AwsOptions
{
    [CommandSwitch("--fleet-name")]
    public string? FleetName { get; set; }

    [CommandSwitch("--application-arn")]
    public string? ApplicationArn { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}