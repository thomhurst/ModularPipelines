using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("blueprint", "version", "artifact", "list")]
public record AzBlueprintVersionArtifactListOptions(
[property: CommandSwitch("--blueprint-name")] string BlueprintName,
[property: CommandSwitch("--version")] string Version
) : AzOptions
{
    [CommandSwitch("--management-group")]
    public string? ManagementGroup { get; set; }

    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}