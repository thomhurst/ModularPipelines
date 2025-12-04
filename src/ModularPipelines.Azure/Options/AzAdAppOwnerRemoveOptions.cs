using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "app", "owner", "remove")]
public record AzAdAppOwnerRemoveOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--owner-object-id")] string OwnerObjectId
) : AzOptions;