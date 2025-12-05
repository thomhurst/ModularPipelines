using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "start-task-contact")]
public record AwsConnectStartTaskContactOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--previous-contact-id")]
    public string? PreviousContactId { get; set; }

    [CliOption("--contact-flow-id")]
    public string? ContactFlowId { get; set; }

    [CliOption("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CliOption("--references")]
    public IEnumerable<KeyValue>? References { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--scheduled-time")]
    public long? ScheduledTime { get; set; }

    [CliOption("--task-template-id")]
    public string? TaskTemplateId { get; set; }

    [CliOption("--quick-connect-id")]
    public string? QuickConnectId { get; set; }

    [CliOption("--related-contact-id")]
    public string? RelatedContactId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}