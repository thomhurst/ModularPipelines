using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "ssl-certificates", "create")]
public record GcloudAppSslCertificatesCreateOptions(
[property: CliOption("--certificate")] string Certificate,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--private-key")] string PrivateKey
) : GcloudOptions;