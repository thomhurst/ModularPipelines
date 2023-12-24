using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearchserverless", "create-vpc-endpoint")]
public record AwsOpensearchserverlessCreateVpcEndpointOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--subnet-ids")] string[] SubnetIds,
[property: CommandSwitch("--vpc-id")] string VpcId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}