using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("postgres", "flexible-server", "list-skus")]
public record AzPostgresFlexibleServerListSkusOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;