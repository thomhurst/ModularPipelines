using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "owner", "add")]
public record AzAdAppOwnerAddOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--owner-object-id")] string OwnerObjectId
) : AzOptions;