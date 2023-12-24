using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "create-resolver-endpoint")]
public record AwsRoute53resolverCreateResolverEndpointOptions(
[property: CommandSwitch("--creator-request-id")] string CreatorRequestId,
[property: CommandSwitch("--security-group-ids")] string[] SecurityGroupIds,
[property: CommandSwitch("--direction")] string Direction,
[property: CommandSwitch("--ip-addresses")] string[] IpAddresses
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--outpost-arn")]
    public string? OutpostArn { get; set; }

    [CommandSwitch("--preferred-instance-type")]
    public string? PreferredInstanceType { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--resolver-endpoint-type")]
    public string? ResolverEndpointType { get; set; }

    [CommandSwitch("--protocols")]
    public string[]? Protocols { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}