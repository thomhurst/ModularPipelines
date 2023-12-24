using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "start-task-contact")]
public record AwsConnectStartTaskContactOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--previous-contact-id")]
    public string? PreviousContactId { get; set; }

    [CommandSwitch("--contact-flow-id")]
    public string? ContactFlowId { get; set; }

    [CommandSwitch("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CommandSwitch("--references")]
    public IEnumerable<KeyValue>? References { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--scheduled-time")]
    public long? ScheduledTime { get; set; }

    [CommandSwitch("--task-template-id")]
    public string? TaskTemplateId { get; set; }

    [CommandSwitch("--quick-connect-id")]
    public string? QuickConnectId { get; set; }

    [CommandSwitch("--related-contact-id")]
    public string? RelatedContactId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}