using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("nginx", "deployment", "certificate", "show")]
public record AzNginxDeploymentCertificateShowOptions : AzOptions
{
    [CliOption("--certificate-name")]
    public string? CertificateName { get; set; }

    [CliOption("--deployment-name")]
    public string? DeploymentName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}