using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "update-lag")]
public record AwsDirectconnectUpdateLagOptions(
[property: CliOption("--lag-id")] string LagId
) : AwsOptions
{
    [CliOption("--lag-name")]
    public string? LagName { get; set; }

    [CliOption("--minimum-links")]
    public int? MinimumLinks { get; set; }

    [CliOption("--encryption-mode")]
    public string? EncryptionMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}