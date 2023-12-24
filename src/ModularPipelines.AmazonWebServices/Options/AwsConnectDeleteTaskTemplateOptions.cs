using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "delete-task-template")]
public record AwsConnectDeleteTaskTemplateOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--task-template-id")] string TaskTemplateId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}