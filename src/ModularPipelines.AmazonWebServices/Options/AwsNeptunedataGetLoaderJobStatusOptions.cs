using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptunedata", "get-loader-job-status")]
public record AwsNeptunedataGetLoaderJobStatusOptions(
[property: CliOption("--load-id")] string LoadId
) : AwsOptions
{
    [CliOption("--page")]
    public int? Page { get; set; }

    [CliOption("--errors-per-page")]
    public int? ErrorsPerPage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}