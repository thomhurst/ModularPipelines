using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "zones", "actions", "list")]
public record GcloudDataplexZonesActionsListOptions(
[property: CommandSwitch("--zone")] string Zone,
[property: CommandSwitch("--lake")] string Lake,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;