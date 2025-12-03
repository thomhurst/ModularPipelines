using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("assured", "workloads", "update")]
public record GcloudAssuredWorkloadsUpdateOptions(
[property: CliArgument] string Workload,
[property: CliArgument] string Location,
[property: CliArgument] string Organization,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--labels")] IEnumerable<KeyValue> Labels,
[property: CliOption("--violation-notifications-enabled")] string ViolationNotificationsEnabled
) : GcloudOptions
{
    [CliOption("--etag")]
    public string? Etag { get; set; }
}