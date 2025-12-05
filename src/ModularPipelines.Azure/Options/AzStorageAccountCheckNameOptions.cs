using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "account", "check-name")]
public record AzStorageAccountCheckNameOptions(
[property: CliOption("--name")] string Name
) : AzOptions;