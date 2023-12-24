using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "disassociate-qualification-from-worker")]
public record AwsMturkDisassociateQualificationFromWorkerOptions(
[property: CommandSwitch("--worker-id")] string WorkerId,
[property: CommandSwitch("--qualification-type-id")] string QualificationTypeId
) : AwsOptions
{
    [CommandSwitch("--reason")]
    public string? Reason { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}