using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "list-vpc-association-authorizations")]
public record AwsRoute53ListVpcAssociationAuthorizationsOptions(
[property: CommandSwitch("--hosted-zone-id")] string HostedZoneId
) : AwsOptions
{
    [CommandSwitch("--max-results")]
    public string? MaxResults { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}