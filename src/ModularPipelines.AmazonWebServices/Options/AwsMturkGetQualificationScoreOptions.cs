using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mturk", "get-qualification-score")]
public record AwsMturkGetQualificationScoreOptions(
[property: CliOption("--qualification-type-id")] string QualificationTypeId,
[property: CliOption("--worker-id")] string WorkerId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}