using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("tag", "update")]
public record AzTagUpdateOptions(
[property: CliOption("--operation")] string Operation,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--tags")] string Tags
) : AzOptions;