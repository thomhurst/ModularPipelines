using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "group", "owner", "remove")]
public record AzAdGroupOwnerRemoveOptions(
[property: CliOption("--group")] string Group,
[property: CliOption("--owner-object-id")] string OwnerObjectId
) : AzOptions;