using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("account", "alias", "delete")]
public record AzAccountAliasDeleteOptions(
[property: CliOption("--name")] string Name
) : AzOptions;