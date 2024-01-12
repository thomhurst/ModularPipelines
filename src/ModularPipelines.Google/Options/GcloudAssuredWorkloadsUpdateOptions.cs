using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("assured", "workloads", "update")]
public record GcloudAssuredWorkloadsUpdateOptions(
[property: PositionalArgument] string Workload,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Organization,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--labels")] IEnumerable<KeyValue> Labels,
[property: CommandSwitch("--violation-notifications-enabled")] string ViolationNotificationsEnabled
) : GcloudOptions
{
    [CommandSwitch("--etag")]
    public string? Etag { get; set; }
}