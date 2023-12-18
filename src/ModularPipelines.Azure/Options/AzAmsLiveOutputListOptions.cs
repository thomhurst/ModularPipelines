using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "live-output", "list")]
public record AzAmsLiveOutputListOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--live-event-name")] string LiveEventName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}