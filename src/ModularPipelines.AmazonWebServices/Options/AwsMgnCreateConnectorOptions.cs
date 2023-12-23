using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgn", "create-connector")]
public record AwsMgnCreateConnectorOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--ssm-instance-id")] string SsmInstanceId
) : AwsOptions
{
    [CommandSwitch("--ssm-command-config")]
    public string? SsmCommandConfig { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}