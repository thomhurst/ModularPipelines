using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "update-expiration-for-hit")]
public record AwsMturkUpdateExpirationForHitOptions(
[property: CommandSwitch("--hit-id")] string HitId,
[property: CommandSwitch("--expire-at")] long ExpireAt
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}