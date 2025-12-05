using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "describe-agent-versions")]
public record AwsOpsworksDescribeAgentVersionsOptions : AwsOptions
{
    [CliOption("--stack-id")]
    public string? StackId { get; set; }

    [CliOption("--configuration-manager")]
    public string? ConfigurationManager { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}