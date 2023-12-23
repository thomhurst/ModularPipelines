using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "create-qualification-type")]
public record AwsMturkCreateQualificationTypeOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--qualification-type-status")] string QualificationTypeStatus
) : AwsOptions
{
    [CommandSwitch("--keywords")]
    public string? Keywords { get; set; }

    [CommandSwitch("--retry-delay-in-seconds")]
    public long? RetryDelayInSeconds { get; set; }

    [CommandSwitch("--test")]
    public string? Test { get; set; }

    [CommandSwitch("--answer-key")]
    public string? AnswerKey { get; set; }

    [CommandSwitch("--test-duration-in-seconds")]
    public long? TestDurationInSeconds { get; set; }

    [CommandSwitch("--auto-granted-value")]
    public int? AutoGrantedValue { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}