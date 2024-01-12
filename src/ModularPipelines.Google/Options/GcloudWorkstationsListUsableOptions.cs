using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workstations", "list-usable")]
public record GcloudWorkstationsListUsableOptions(
[property: CommandSwitch("--config")] string Config,
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;