using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "project", "task", "delete")]
public record AzDmsProjectTaskDeleteOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions
{
    [CliOption("--delete-running-tasks")]
    public string? DeleteRunningTasks { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}