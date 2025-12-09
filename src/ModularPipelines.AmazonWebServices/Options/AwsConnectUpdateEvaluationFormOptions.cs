using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-evaluation-form")]
public record AwsConnectUpdateEvaluationFormOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--evaluation-form-id")] string EvaluationFormId,
[property: CliOption("--evaluation-form-version")] int EvaluationFormVersion,
[property: CliOption("--title")] string Title,
[property: CliOption("--items")] string[] Items
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--scoring-strategy")]
    public string? ScoringStrategy { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}