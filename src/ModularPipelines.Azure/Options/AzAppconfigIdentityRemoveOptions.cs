using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfig", "identity", "remove")]
public record AzAppconfigIdentityRemoveOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--identities")]
    public string? Identities { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}