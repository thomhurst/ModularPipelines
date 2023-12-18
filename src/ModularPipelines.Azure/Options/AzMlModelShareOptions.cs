using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "model", "share")]
public record AzMlModelShareOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry-name")] string RegistryName,
[property: CommandSwitch("--share-with-name")] string ShareWithName,
[property: CommandSwitch("--share-with-version")] string ShareWithVersion,
[property: CommandSwitch("--version")] string Version
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}