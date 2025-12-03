using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("new-relic", "account", "list")]
public record AzNewRelicAccountListOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--user-email")] string UserEmail
) : AzOptions;