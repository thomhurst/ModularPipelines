using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "deactivate-evaluation-form")]
public record AwsConnectDeactivateEvaluationFormOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--evaluation-form-id")] string EvaluationFormId,
[property: CliOption("--evaluation-form-version")] int EvaluationFormVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}