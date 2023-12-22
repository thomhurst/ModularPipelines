using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "group", "owner", "list")]
public record AzAdGroupOwnerListOptions(
[property: CommandSwitch("--group")] string Group
) : AzOptions;