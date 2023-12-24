using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acm", "put-account-configuration")]
public record AwsAcmPutAccountConfigurationOptions(
[property: CommandSwitch("--idempotency-token")] string IdempotencyToken
) : AwsOptions
{
    [CommandSwitch("--expiry-events")]
    public string? ExpiryEvents { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}