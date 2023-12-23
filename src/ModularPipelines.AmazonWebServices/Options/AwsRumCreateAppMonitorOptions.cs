using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rum", "create-app-monitor")]
public record AwsRumCreateAppMonitorOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--app-monitor-configuration")]
    public string? AppMonitorConfiguration { get; set; }

    [CommandSwitch("--custom-events")]
    public string? CustomEvents { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}