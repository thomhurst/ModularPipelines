using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "update-trust")]
public record AwsDsUpdateTrustOptions(
[property: CliOption("--trust-id")] string TrustId
) : AwsOptions
{
    [CliOption("--selective-auth")]
    public string? SelectiveAuth { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}