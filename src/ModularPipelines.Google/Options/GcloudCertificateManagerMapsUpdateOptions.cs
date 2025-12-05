using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "maps", "update")]
public record GcloudCertificateManagerMapsUpdateOptions(
[property: CliArgument] string Map,
[property: CliArgument] string Location
) : GcloudOptions;