using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "create-resolver-endpoint")]
public record AwsRoute53resolverCreateResolverEndpointOptions(
[property: CliOption("--creator-request-id")] string CreatorRequestId,
[property: CliOption("--security-group-ids")] string[] SecurityGroupIds,
[property: CliOption("--direction")] string Direction,
[property: CliOption("--ip-addresses")] string[] IpAddresses
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--outpost-arn")]
    public string? OutpostArn { get; set; }

    [CliOption("--preferred-instance-type")]
    public string? PreferredInstanceType { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--resolver-endpoint-type")]
    public string? ResolverEndpointType { get; set; }

    [CliOption("--protocols")]
    public string[]? Protocols { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}