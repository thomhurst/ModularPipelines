using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "env", "certificate", "create")]
public record AzContainerappEnvCertificateCreateOptions(
[property: CliOption("--hostname")] string Hostname,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--validation-method")] string ValidationMethod
) : AzOptions
{
    [CliOption("--certificate-name")]
    public string? CertificateName { get; set; }
}