using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appflow", "cancel-flow-executions")]
public record AwsAppflowCancelFlowExecutionsOptions(
[property: CliOption("--flow-name")] string FlowName
) : AwsOptions
{
    [CliOption("--execution-ids")]
    public string[]? ExecutionIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}