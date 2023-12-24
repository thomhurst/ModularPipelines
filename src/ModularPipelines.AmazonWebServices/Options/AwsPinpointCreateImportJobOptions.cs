using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "create-import-job")]
public record AwsPinpointCreateImportJobOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--import-job-request")] string ImportJobRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}