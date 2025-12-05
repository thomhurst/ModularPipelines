using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "update-job")]
public record AwsGlueUpdateJobOptions(
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--job-update")] string JobUpdate
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}