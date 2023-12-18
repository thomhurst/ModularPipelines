using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("new-relic", "organization", "list")]
public record AzNewRelicOrganizationListOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--user-email")] string UserEmail
) : AzOptions
{
}