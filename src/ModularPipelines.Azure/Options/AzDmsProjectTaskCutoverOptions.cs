using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "project", "task", "cutover")]
public record AzDmsProjectTaskCutoverOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--object-name")] string ObjectName,
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions;