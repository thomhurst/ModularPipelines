using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "app", "owner", "add")]
public record AzAdAppOwnerAddOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--owner-object-id")] string OwnerObjectId
) : AzOptions;