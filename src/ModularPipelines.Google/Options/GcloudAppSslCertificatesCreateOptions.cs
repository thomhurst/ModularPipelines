using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "ssl-certificates", "create")]
public record GcloudAppSslCertificatesCreateOptions(
[property: CommandSwitch("--certificate")] string Certificate,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--private-key")] string PrivateKey
) : GcloudOptions;