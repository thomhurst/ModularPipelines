using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "describe-agent-versions")]
public record AwsOpsworksDescribeAgentVersionsOptions : AwsOptions
{
    [CommandSwitch("--stack-id")]
    public string? StackId { get; set; }

    [CommandSwitch("--configuration-manager")]
    public string? ConfigurationManager { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}