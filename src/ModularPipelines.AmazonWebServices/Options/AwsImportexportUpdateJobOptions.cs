using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("importexport", "update-job")]
public record AwsImportexportUpdateJobOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--manifest")] string Manifest,
[property: CliOption("--job-type")] string JobType
) : AwsOptions
{
    [CliOption("--api-version")]
    public string? ApiVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}