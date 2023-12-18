using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcappliance", "prepare", "scvmm")]
public record AzArcappliancePrepareScvmmOptions(
[property: CommandSwitch("--config-file")] string ConfigFile
) : AzOptions
{
    [CommandSwitch("--address")]
    public string? Address { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }
}