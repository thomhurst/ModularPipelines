using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acm", "put-account-configuration")]
public record AwsAcmPutAccountConfigurationOptions(
[property: CliOption("--idempotency-token")] string IdempotencyToken
) : AwsOptions
{
    [CliOption("--expiry-events")]
    public string? ExpiryEvents { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}