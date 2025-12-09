using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mturk", "create-qualification-type")]
public record AwsMturkCreateQualificationTypeOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--description")] string Description,
[property: CliOption("--qualification-type-status")] string QualificationTypeStatus
) : AwsOptions
{
    [CliOption("--keywords")]
    public string? Keywords { get; set; }

    [CliOption("--retry-delay-in-seconds")]
    public long? RetryDelayInSeconds { get; set; }

    [CliOption("--test")]
    public string? Test { get; set; }

    [CliOption("--answer-key")]
    public string? AnswerKey { get; set; }

    [CliOption("--test-duration-in-seconds")]
    public long? TestDurationInSeconds { get; set; }

    [CliOption("--auto-granted-value")]
    public int? AutoGrantedValue { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}