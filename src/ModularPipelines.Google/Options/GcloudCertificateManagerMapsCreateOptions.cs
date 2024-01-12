using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("certificate-manager", "maps", "create")]
public record GcloudCertificateManagerMapsCreateOptions(
[property: PositionalArgument] string Map,
[property: PositionalArgument] string Location
) : GcloudOptions;