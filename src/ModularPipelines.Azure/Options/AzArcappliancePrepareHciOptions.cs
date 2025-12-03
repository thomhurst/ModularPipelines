using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arcappliance", "prepare", "hci")]
public record AzArcappliancePrepareHciOptions(
[property: CliOption("--config-file")] string ConfigFile
) : AzOptions
{
    [CliOption("--cloudagent")]
    public string? Cloudagent { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--loginconfigfile")]
    public string? Loginconfigfile { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}