using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("importexport", "create-job")]
public record AwsImportexportCreateJobOptions(
[property: CliOption("--job-type")] string JobType,
[property: CliOption("--manifest")] string Manifest
) : AwsOptions
{
    [CliOption("--manifest-addendum")]
    public string? ManifestAddendum { get; set; }

    [CliOption("--api-version")]
    public string? ApiVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}