using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "recoverable-midb", "list")]
public record AzSqlRecoverableMidbListOptions(
[property: CommandSwitch("--instance-name")] string InstanceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;