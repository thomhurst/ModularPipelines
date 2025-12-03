using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arcappliance", "deploy", "scvmm")]
public record AzArcapplianceDeployScvmmOptions(
[property: CliOption("--config-file")] string ConfigFile
) : AzOptions
{
    [CliOption("--address")]
    public string? Address { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--outfile")]
    public string? Outfile { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}