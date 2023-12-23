using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "update-qualification-type")]
public record AwsMturkUpdateQualificationTypeOptions(
[property: CommandSwitch("--qualification-type-id")] string QualificationTypeId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--qualification-type-status")]
    public string? QualificationTypeStatus { get; set; }

    [CommandSwitch("--test")]
    public string? Test { get; set; }

    [CommandSwitch("--answer-key")]
    public string? AnswerKey { get; set; }

    [CommandSwitch("--test-duration-in-seconds")]
    public long? TestDurationInSeconds { get; set; }

    [CommandSwitch("--retry-delay-in-seconds")]
    public long? RetryDelayInSeconds { get; set; }

    [CommandSwitch("--auto-granted-value")]
    public int? AutoGrantedValue { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}