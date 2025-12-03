using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alias", "remove")]
public record AzAliasRemoveOptions(
[property: CliOption("--name")] string Name
) : AzOptions;