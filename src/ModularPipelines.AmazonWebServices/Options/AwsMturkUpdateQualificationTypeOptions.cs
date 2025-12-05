using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mturk", "update-qualification-type")]
public record AwsMturkUpdateQualificationTypeOptions(
[property: CliOption("--qualification-type-id")] string QualificationTypeId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--qualification-type-status")]
    public string? QualificationTypeStatus { get; set; }

    [CliOption("--test")]
    public string? Test { get; set; }

    [CliOption("--answer-key")]
    public string? AnswerKey { get; set; }

    [CliOption("--test-duration-in-seconds")]
    public long? TestDurationInSeconds { get; set; }

    [CliOption("--retry-delay-in-seconds")]
    public long? RetryDelayInSeconds { get; set; }

    [CliOption("--auto-granted-value")]
    public int? AutoGrantedValue { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}