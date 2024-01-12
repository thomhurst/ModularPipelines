using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "zones", "list")]
public record GcloudDataplexZonesListOptions(
[property: CommandSwitch("--lake")] string Lake,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;