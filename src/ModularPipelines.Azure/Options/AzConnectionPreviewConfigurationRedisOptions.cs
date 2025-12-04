using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("connection", "preview-configuration", "redis")]
public record AzConnectionPreviewConfigurationRedisOptions : AzOptions
{
    [CliOption("--client-type")]
    public string? ClientType { get; set; }

    [CliOption("--secret")]
    public string? Secret { get; set; }
}