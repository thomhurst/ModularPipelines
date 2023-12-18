using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "owner", "add")]
public record AzAdAppOwnerAddOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--owner-object-id")] string OwnerObjectId
) : AzOptions
{
}

