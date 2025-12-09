using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "group", "owner", "add")]
public record AzAdGroupOwnerAddOptions(
[property: CliOption("--group")] string Group,
[property: CliOption("--owner-object-id")] string OwnerObjectId
) : AzOptions;