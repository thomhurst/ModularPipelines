using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "app", "credential", "reset")]
public record AzAdAppCredentialResetOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliFlag("--append")]
    public bool? Append { get; set; }

    [CliOption("--cert")]
    public string? Cert { get; set; }

    [CliFlag("--create-cert")]
    public bool? CreateCert { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--end-date")]
    public string? EndDate { get; set; }

    [CliOption("--keyvault")]
    public string? Keyvault { get; set; }

    [CliOption("--years")]
    public string? Years { get; set; }
}