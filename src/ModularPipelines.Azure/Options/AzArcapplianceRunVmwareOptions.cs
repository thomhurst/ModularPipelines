using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("arcappliance", "run", "vmware")]
public record AzArcapplianceRunVmwareOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--address")]
    public string? Address { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--out-dir")]
    public string? OutDir { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }

    [CliOption("--working-dir")]
    public string? WorkingDir { get; set; }
}