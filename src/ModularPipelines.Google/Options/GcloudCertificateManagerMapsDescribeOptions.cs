using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("certificate-manager", "maps", "describe")]
public record GcloudCertificateManagerMapsDescribeOptions(
[property: PositionalArgument] string Map,
[property: PositionalArgument] string Location
) : GcloudOptions;