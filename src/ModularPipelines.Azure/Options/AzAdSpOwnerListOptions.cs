using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "sp", "owner", "list")]
public record AzAdSpOwnerListOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions;