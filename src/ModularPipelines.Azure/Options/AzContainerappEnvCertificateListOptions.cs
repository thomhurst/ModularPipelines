using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "env", "certificate", "list")]
public record AzContainerappEnvCertificateListOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--certificate")]
    public string? Certificate { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--managed-certificates-only")]
    public bool? ManagedCertificatesOnly { get; set; }

    [CliFlag("--private-key-certificates-only")]
    public bool? PrivateKeyCertificatesOnly { get; set; }

    [CliOption("--thumbprint")]
    public string? Thumbprint { get; set; }
}