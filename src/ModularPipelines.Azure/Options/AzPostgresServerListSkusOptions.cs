using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("postgres", "server", "list-skus")]
public record AzPostgresServerListSkusOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;