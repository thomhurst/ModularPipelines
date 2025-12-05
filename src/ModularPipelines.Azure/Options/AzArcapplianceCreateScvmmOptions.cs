using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("arcappliance", "create", "scvmm")]
public record AzArcapplianceCreateScvmmOptions(
[property: CliOption("--config-file")] string ConfigFile,
[property: CliOption("--kubeconfig")] string Kubeconfig
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}