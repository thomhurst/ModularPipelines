using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "get-task-template")]
public record AwsConnectGetTaskTemplateOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--task-template-id")] string TaskTemplateId
) : AwsOptions
{
    [CliOption("--snapshot-version")]
    public string? SnapshotVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}