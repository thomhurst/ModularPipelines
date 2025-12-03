using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "connected-env", "certificate", "list")]
public record AzContainerappConnectedEnvCertificateListOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--certificate")]
    public string? Certificate { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--thumbprint")]
    public string? Thumbprint { get; set; }
}