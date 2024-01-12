using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "assets", "actions", "list")]
public record GcloudDataplexAssetsActionsListOptions(
[property: CommandSwitch("--asset")] string Asset,
[property: CommandSwitch("--lake")] string Lake,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--zone")] string Zone
) : GcloudOptions;