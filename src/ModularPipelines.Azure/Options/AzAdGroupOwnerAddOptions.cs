using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "group", "owner", "add")]
public record AzAdGroupOwnerAddOptions(
[property: CommandSwitch("--group")] string Group,
[property: CommandSwitch("--owner-object-id")] string OwnerObjectId
) : AzOptions;