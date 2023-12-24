using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "create-export-job")]
public record AwsPinpointCreateExportJobOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--export-job-request")] string ExportJobRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}