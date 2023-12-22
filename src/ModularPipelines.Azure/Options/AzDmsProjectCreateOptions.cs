using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "project", "create")]
public record AzDmsProjectCreateOptions(
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