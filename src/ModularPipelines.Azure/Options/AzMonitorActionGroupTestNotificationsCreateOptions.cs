using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "action-group", "test-notifications", "create")]
public record AzMonitorActionGroupTestNotificationsCreateOptions(
[property: CliOption("--action-group")] string ActionGroup,
[property: CliOption("--alert-type")] string AlertType,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--add-action")]
    public string? AddAction { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}