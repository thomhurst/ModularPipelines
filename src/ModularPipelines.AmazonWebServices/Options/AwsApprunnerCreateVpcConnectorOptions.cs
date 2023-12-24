using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apprunner", "create-vpc-connector")]
public record AwsApprunnerCreateVpcConnectorOptions(
[property: CommandSwitch("--vpc-connector-name")] string VpcConnectorName,
[property: CommandSwitch("--subnets")] string[] Subnets
) : AwsOptions
{
    [CommandSwitch("--security-groups")]
    public string[]? SecurityGroups { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}