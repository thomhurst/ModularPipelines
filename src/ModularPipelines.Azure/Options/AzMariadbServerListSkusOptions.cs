using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mariadb", "server", "list-skus")]
public record AzMariadbServerListSkusOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;