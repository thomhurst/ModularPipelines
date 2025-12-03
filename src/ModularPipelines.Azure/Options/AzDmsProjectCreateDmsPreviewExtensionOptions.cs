using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "project", "create", "(dms-preview", "extension)")]
public record AzDmsProjectCreateDmsPreviewExtensionOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName,
[property: CliOption("--source-platform")] string SourcePlatform,
[property: CliOption("--target-platform")] string TargetPlatform
) : AzOptions
{
    [CliOption("--tags")]
    public string? Tags { get; set; }
}