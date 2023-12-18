using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("new-relic", "account", "list")]
public record AzNewRelicAccountListOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--user-email")] string UserEmail
) : AzOptions;