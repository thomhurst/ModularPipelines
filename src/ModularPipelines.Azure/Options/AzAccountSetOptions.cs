using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("account", "set")]
public record AzAccountSetOptions(
[property: CliOption("--name")] string Name
) : AzOptions;