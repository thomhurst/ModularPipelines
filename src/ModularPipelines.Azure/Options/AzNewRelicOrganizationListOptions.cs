using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("new-relic", "organization", "list")]
public record AzNewRelicOrganizationListOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--user-email")] string UserEmail
) : AzOptions;