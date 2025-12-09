using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "certificate", "get-default-policy")]
public record AzKeyvaultCertificateGetDefaultPolicyOptions : AzOptions
{
    [CliFlag("--scaffold")]
    public bool? Scaffold { get; set; }
}