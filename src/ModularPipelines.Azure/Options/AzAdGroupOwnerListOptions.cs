using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "group", "owner", "list")]
public record AzAdGroupOwnerListOptions(
[property: CliOption("--group")] string Group
) : AzOptions;