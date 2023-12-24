using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearchserverless", "update-vpc-endpoint")]
public record AwsOpensearchserverlessUpdateVpcEndpointOptions(
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--add-security-group-ids")]
    public string[]? AddSecurityGroupIds { get; set; }

    [CommandSwitch("--add-subnet-ids")]
    public string[]? AddSubnetIds { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--remove-security-group-ids")]
    public string[]? RemoveSecurityGroupIds { get; set; }

    [CommandSwitch("--remove-subnet-ids")]
    public string[]? RemoveSubnetIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}