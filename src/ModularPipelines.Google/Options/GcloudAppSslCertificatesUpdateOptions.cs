using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "ssl-certificates", "update")]
public record GcloudAppSslCertificatesUpdateOptions(
[property: CliArgument] string Id
) : GcloudOptions
{
    [CliOption("--certificate")]
    public string? Certificate { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--private-key")]
    public string? PrivateKey { get; set; }
}