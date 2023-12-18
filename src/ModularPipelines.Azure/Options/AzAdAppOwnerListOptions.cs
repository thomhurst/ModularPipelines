using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "owner", "list")]
public record AzAdAppOwnerListOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions;