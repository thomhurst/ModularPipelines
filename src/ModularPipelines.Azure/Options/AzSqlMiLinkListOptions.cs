using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi", "link", "list")]
public record AzSqlMiLinkListOptions(
[property: CommandSwitch("--instance-name")] string InstanceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;