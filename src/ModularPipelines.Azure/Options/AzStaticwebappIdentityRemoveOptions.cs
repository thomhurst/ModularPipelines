using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("staticwebapp", "identity", "remove")]
public record AzStaticwebappIdentityRemoveOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--identities")]
    public string? Identities { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}