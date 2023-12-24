using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "update-vpc-connection")]
public record AwsQuicksightUpdateVpcConnectionOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--vpc-connection-id")] string VpcConnectionId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--subnet-ids")] string[] SubnetIds,
[property: CommandSwitch("--security-group-ids")] string[] SecurityGroupIds,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--dns-resolvers")]
    public string[]? DnsResolvers { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}