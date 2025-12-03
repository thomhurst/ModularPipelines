using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "create-export-job")]
public record AwsPinpointCreateExportJobOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--export-job-request")] string ExportJobRequest
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}