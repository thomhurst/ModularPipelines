using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyvault", "certificate", "get-default-policy")]
public record AzKeyvaultCertificateGetDefaultPolicyOptions : AzOptions
{
    [CliFlag("--scaffold")]
    public bool? Scaffold { get; set; }
}