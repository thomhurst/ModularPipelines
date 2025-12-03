using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mturk", "update-hit-type-of-hit")]
public record AwsMturkUpdateHitTypeOfHitOptions(
[property: CliOption("--hit-id")] string HitId,
[property: CliOption("--hit-type-id")] string HitTypeId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}