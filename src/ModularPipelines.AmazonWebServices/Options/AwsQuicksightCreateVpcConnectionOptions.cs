using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "create-vpc-connection")]
public record AwsQuicksightCreateVpcConnectionOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--vpc-connection-id")] string VpcConnectionId,
[property: CliOption("--name")] string Name,
[property: CliOption("--subnet-ids")] string[] SubnetIds,
[property: CliOption("--security-group-ids")] string[] SecurityGroupIds,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--dns-resolvers")]
    public string[]? DnsResolvers { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}