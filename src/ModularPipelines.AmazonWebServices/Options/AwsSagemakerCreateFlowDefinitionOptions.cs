using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-flow-definition")]
public record AwsSagemakerCreateFlowDefinitionOptions(
[property: CliOption("--flow-definition-name")] string FlowDefinitionName,
[property: CliOption("--output-config")] string OutputConfig,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--human-loop-request-source")]
    public string? HumanLoopRequestSource { get; set; }

    [CliOption("--human-loop-activation-config")]
    public string? HumanLoopActivationConfig { get; set; }

    [CliOption("--human-loop-config")]
    public string? HumanLoopConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}