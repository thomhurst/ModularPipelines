using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("certificate-manager", "maps", "entries", "list")]
public record GcloudCertificateManagerMapsEntriesListOptions(
[property: CommandSwitch("--map")] string Map,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;