using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "project", "create", "(dms-preview", "extension)")]
public record AzDmsProjectCreateDmsPreviewExtensionOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName,
[property: CommandSwitch("--source-platform")] string SourcePlatform,
[property: CommandSwitch("--target-platform")] string TargetPlatform
) : AzOptions
{
    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}