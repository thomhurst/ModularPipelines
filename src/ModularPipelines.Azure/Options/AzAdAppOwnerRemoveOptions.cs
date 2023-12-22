using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "owner", "remove")]
public record AzAdAppOwnerRemoveOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--owner-object-id")] string OwnerObjectId
) : AzOptions;