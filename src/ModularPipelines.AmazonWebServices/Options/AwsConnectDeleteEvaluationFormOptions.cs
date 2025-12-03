using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "delete-evaluation-form")]
public record AwsConnectDeleteEvaluationFormOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--evaluation-form-id")] string EvaluationFormId
) : AwsOptions
{
    [CliOption("--evaluation-form-version")]
    public int? EvaluationFormVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}