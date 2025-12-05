using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "create-import-job")]
public record AwsPinpointCreateImportJobOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--import-job-request")] string ImportJobRequest
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}