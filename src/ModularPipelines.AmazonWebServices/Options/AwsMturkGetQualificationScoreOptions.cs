using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "get-qualification-score")]
public record AwsMturkGetQualificationScoreOptions(
[property: CommandSwitch("--qualification-type-id")] string QualificationTypeId,
[property: CommandSwitch("--worker-id")] string WorkerId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}