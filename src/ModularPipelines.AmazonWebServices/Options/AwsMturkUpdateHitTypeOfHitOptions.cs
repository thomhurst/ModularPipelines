using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "update-hit-type-of-hit")]
public record AwsMturkUpdateHitTypeOfHitOptions(
[property: CommandSwitch("--hit-id")] string HitId,
[property: CommandSwitch("--hit-type-id")] string HitTypeId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}