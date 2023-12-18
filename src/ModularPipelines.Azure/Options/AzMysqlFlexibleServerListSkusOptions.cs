using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mysql", "flexible-server", "list-skus")]
public record AzMysqlFlexibleServerListSkusOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;