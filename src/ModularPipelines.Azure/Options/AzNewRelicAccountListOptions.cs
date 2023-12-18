using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("new-relic", "account", "list")]
public record AzNewRelicAccountListOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--user-email")] string UserEmail
) : AzOptions
{
}