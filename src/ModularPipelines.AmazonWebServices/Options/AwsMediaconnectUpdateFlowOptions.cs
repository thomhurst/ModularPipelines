using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "update-flow")]
public record AwsMediaconnectUpdateFlowOptions(
[property: CliOption("--flow-arn")] string FlowArn
) : AwsOptions
{
    [CliOption("--source-failover-config")]
    public string? SourceFailoverConfig { get; set; }

    [CliOption("--maintenance")]
    public string? Maintenance { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}