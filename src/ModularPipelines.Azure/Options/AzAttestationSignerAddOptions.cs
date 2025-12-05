using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("attestation", "signer", "add")]
public record AzAttestationSignerAddOptions : AzOptions
{
    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--signer")]
    public string? Signer { get; set; }

    [CliOption("--signer-file")]
    public string? SignerFile { get; set; }
}