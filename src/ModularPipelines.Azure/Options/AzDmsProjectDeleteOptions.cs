using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dms", "project", "delete")]
public record AzDmsProjectDeleteOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions
{
    [CliOption("--delete-running-tasks")]
    public string? DeleteRunningTasks { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}