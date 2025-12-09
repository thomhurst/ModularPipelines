using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "create-framework")]
public record AwsBackupCreateFrameworkOptions(
[property: CliOption("--framework-name")] string FrameworkName,
[property: CliOption("--framework-controls")] string[] FrameworkControls
) : AwsOptions
{
    [CliOption("--framework-description")]
    public string? FrameworkDescription { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--framework-tags")]
    public IEnumerable<KeyValue>? FrameworkTags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}