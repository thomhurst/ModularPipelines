using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "create-agent-status")]
public record AwsConnectCreateAgentStatusOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--state")] string State
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-order")]
    public int? DisplayOrder { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}