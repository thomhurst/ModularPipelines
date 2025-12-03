using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("account", "alias", "show")]
public record AzAccountAliasShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;