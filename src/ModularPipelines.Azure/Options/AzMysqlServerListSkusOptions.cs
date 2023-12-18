using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mysql", "server", "list-skus")]
public record AzMysqlServerListSkusOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;