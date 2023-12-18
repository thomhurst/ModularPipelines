using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "data", "share")]
public record AzMlDataShareOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry-name")] string RegistryName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--share-with-name")] string ShareWithName,
[property: CommandSwitch("--share-with-version")] string ShareWithVersion,
[property: CommandSwitch("--version")] string Version,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--label")]
    public string? Label { get; set; }
}

