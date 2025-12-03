using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "maps", "entries", "list")]
public record GcloudCertificateManagerMapsEntriesListOptions(
[property: CliOption("--map")] string Map,
[property: CliOption("--location")] string Location
) : GcloudOptions;