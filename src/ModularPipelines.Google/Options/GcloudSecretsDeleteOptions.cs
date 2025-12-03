using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secrets", "delete")]
public record GcloudSecretsDeleteOptions(
[property: CliArgument] string Secret
) : GcloudOptions
{
    [CliOption("--etag")]
    public string? Etag { get; set; }
}