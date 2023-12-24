using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "associate-qualification-with-worker")]
public record AwsMturkAssociateQualificationWithWorkerOptions(
[property: CommandSwitch("--qualification-type-id")] string QualificationTypeId,
[property: CommandSwitch("--worker-id")] string WorkerId
) : AwsOptions
{
    [CommandSwitch("--integer-value")]
    public int? IntegerValue { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}