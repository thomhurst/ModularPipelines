using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "project", "task", "cancel", "(dms-preview", "extension)")]
public record AzDmsProjectTaskCancelDmsPreviewExtensionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions
{
    [CliOption("--object-name")]
    public string? ObjectName { get; set; }
}