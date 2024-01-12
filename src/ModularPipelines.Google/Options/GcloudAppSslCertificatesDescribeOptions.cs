using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "ssl-certificates", "describe")]
public record GcloudAppSslCertificatesDescribeOptions(
[property: PositionalArgument] string Id
) : GcloudOptions;