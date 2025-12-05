using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("arcappliance", "delete", "hci")]
public record AzArcapplianceDeleteHciOptions(
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

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}