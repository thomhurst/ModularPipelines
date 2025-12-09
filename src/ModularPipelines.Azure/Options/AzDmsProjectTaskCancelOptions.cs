using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dms", "project", "task", "cancel")]
public record AzDmsProjectTaskCancelOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions;