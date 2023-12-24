using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-task-template")]
public record AwsConnectUpdateTaskTemplateOptions(
[property: CommandSwitch("--task-template-id")] string TaskTemplateId,
[property: CommandSwitch("--instance-id")] string InstanceId
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--contact-flow-id")]
    public string? ContactFlowId { get; set; }

    [CommandSwitch("--constraints")]
    public string? Constraints { get; set; }

    [CommandSwitch("--defaults")]
    public string? Defaults { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--fields")]
    public string[]? Fields { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}