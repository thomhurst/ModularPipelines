using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "identity", "remove")]
public record AzAcrIdentityRemoveOptions(
[property: CommandSwitch("--identities")] string Identities,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}