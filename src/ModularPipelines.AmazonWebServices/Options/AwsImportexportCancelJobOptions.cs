using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("importexport", "cancel-job")]
public record AwsImportexportCancelJobOptions(
[property: CliOption("--job-id")] string JobId
) : AwsOptions
{
    [CliOption("--api-version")]
    public string? ApiVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}