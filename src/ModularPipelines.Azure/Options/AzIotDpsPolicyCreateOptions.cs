using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "dps", "policy", "create")]
public record AzIotDpsPolicyCreateOptions(
[property: CommandSwitch("--dps-name")] string DpsName,
[property: CommandSwitch("--pn")] string Pn,
[property: CommandSwitch("--rights")] string Rights
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--primary-key")]
    public string? PrimaryKey { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--secondary-key")]
    public string? SecondaryKey { get; set; }
}