using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "update-framework")]
public record AwsBackupUpdateFrameworkOptions(
[property: CliOption("--framework-name")] string FrameworkName
) : AwsOptions
{
    [CliOption("--framework-description")]
    public string? FrameworkDescription { get; set; }

    [CliOption("--framework-controls")]
    public string[]? FrameworkControls { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}