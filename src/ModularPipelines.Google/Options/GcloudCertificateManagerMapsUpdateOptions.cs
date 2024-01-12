using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("certificate-manager", "maps", "update")]
public record GcloudCertificateManagerMapsUpdateOptions(
[property: PositionalArgument] string Map,
[property: PositionalArgument] string Location
) : GcloudOptions;