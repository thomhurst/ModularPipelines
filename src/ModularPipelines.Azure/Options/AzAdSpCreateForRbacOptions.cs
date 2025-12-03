using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "sp", "create-for-rbac")]
public record AzAdSpCreateForRbacOptions : AzOptions
{
    [CliOption("--cert")]
    public string? Cert { get; set; }

    [CliFlag("--create-cert")]
    public bool? CreateCert { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--keyvault")]
    public string? Keyvault { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--scopes")]
    public string? Scopes { get; set; }

    [CliOption("--years")]
    public string? Years { get; set; }
}