using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("stack", "group", "list")]
public record AzStackGroupListOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;