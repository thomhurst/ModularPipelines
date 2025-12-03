using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "app", "owner", "list")]
public record AzAdAppOwnerListOptions(
[property: CliOption("--id")] string Id
) : AzOptions;