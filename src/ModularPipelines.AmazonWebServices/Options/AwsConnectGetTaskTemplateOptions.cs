using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "get-task-template")]
public record AwsConnectGetTaskTemplateOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--task-template-id")] string TaskTemplateId
) : AwsOptions
{
    [CommandSwitch("--snapshot-version")]
    public string? SnapshotVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}