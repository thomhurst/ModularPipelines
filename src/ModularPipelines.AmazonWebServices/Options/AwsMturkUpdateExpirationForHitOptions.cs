using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mturk", "update-expiration-for-hit")]
public record AwsMturkUpdateExpirationForHitOptions(
[property: CliOption("--hit-id")] string HitId,
[property: CliOption("--expire-at")] long ExpireAt
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}