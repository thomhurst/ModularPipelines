using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "ssl-certificates", "list")]
public record GcloudAppSslCertificatesListOptions : GcloudOptions;