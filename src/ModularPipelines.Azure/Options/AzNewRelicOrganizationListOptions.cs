using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("new-relic", "organization", "list")]
public record AzNewRelicOrganizationListOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--user-email")] string UserEmail
) : AzOptions;