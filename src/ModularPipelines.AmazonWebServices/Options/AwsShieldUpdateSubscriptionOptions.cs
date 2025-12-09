using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("shield", "update-subscription")]
public record AwsShieldUpdateSubscriptionOptions : AwsOptions
{
    [CliOption("--auto-renew")]
    public string? AutoRenew { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}