using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "maps", "entries", "describe")]
public record GcloudCertificateManagerMapsEntriesDescribeOptions(
[property: CliArgument] string Entry,
[property: CliArgument] string Location,
[property: CliArgument] string Map
) : GcloudOptions;