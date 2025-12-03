using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connection", "preview-configuration", "postgres")]
public record AzConnectionPreviewConfigurationPostgresOptions : AzOptions
{
    [CliOption("--client-type")]
    public string? ClientType { get; set; }

    [CliOption("--secret")]
    public string? Secret { get; set; }

    [CliOption("--user-account")]
    public int? UserAccount { get; set; }
}