using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("connection", "preview-configuration", "cosmos-table")]
public record AzConnectionPreviewConfigurationCosmosTableOptions : AzOptions
{
    [CliOption("--client-type")]
    public string? ClientType { get; set; }

    [CliOption("--secret")]
    public string? Secret { get; set; }

    [CliOption("--service-principal")]
    public string? ServicePrincipal { get; set; }

    [CliOption("--user-account")]
    public int? UserAccount { get; set; }
}