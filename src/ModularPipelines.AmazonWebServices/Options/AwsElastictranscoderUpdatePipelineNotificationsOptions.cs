using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elastictranscoder", "update-pipeline-notifications")]
public record AwsElastictranscoderUpdatePipelineNotificationsOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--notifications")] string Notifications
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}