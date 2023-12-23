using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "list-hosted-zones-by-vpc")]
public record AwsRoute53ListHostedZonesByVpcOptions(
[property: CommandSwitch("--vpc-id")] string VpcId,
[property: CommandSwitch("--vpc-region")] string VpcRegion
) : AwsOptions
{
    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}