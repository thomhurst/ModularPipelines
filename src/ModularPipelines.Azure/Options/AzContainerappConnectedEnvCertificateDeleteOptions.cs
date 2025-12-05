using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "connected-env", "certificate", "delete")]
public record AzContainerappConnectedEnvCertificateDeleteOptions : AzOptions
{
    [CliOption("--certificate")]
    public string? Certificate { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--thumbprint")]
    public string? Thumbprint { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}