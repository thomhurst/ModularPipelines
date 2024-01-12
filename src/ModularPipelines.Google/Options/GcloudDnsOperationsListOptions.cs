using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "operations", "list")]
public record GcloudDnsOperationsListOptions(
[property: CommandSwitch("--zones")] string[] Zones
) : GcloudOptions;