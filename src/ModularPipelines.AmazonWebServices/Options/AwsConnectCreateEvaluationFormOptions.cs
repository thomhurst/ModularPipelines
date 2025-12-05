using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "create-evaluation-form")]
public record AwsConnectCreateEvaluationFormOptions(
[property: CliOption("--instance-id")] string InstanceId,
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