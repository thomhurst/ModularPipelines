using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "service-agent")]
public record GcloudStorageServiceAgentOptions : GcloudOptions
{
    [CliOption("--authorize-cmek")]
    public string? AuthorizeCmek { get; set; }
}