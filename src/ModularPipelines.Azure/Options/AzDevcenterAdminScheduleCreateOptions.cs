using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devcenter", "admin", "schedule", "create")]
public record AzDevcenterAdminScheduleCreateOptions(
[property: CliOption("--pool-name")] string PoolName,
[property: CliOption("--project")] string Project,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--time")] string Time,
[property: CliOption("--time-zone")] string TimeZone
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }
}