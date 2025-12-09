using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dms", "project", "task", "restart")]
public record AzDmsProjectTaskRestartOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions
{
    [CliOption("--object-name")]
    public string? ObjectName { get; set; }
}