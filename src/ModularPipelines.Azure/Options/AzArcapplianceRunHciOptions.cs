using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arcappliance", "run", "hci")]
public record AzArcapplianceRunHciOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--cloudagent")]
    public string? Cloudagent { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--loginconfigfile")]
    public string? Loginconfigfile { get; set; }

    [CliOption("--out-dir")]
    public string? OutDir { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--working-dir")]
    public string? WorkingDir { get; set; }
}