using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "get-job-bookmark")]
public record AwsGlueGetJobBookmarkOptions(
[property: CliOption("--job-name")] string JobName
) : AwsOptions
{
    [CliOption("--run-id")]
    public string? RunId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}