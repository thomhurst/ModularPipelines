using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "ssl-certificates", "delete")]
public record GcloudAppSslCertificatesDeleteOptions(
[property: PositionalArgument] string Id
) : GcloudOptions;