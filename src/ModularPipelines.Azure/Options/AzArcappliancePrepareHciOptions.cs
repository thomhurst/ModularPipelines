using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcappliance", "prepare", "hci")]
public record AzArcappliancePrepareHciOptions(
[property: CommandSwitch("--config-file")] string ConfigFile
) : AzOptions
{
    [CommandSwitch("--cloudagent")]
    public string? Cloudagent { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--loginconfigfile")]
    public string? Loginconfigfile { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}