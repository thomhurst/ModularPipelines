using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("afd", "origin", "list")]
public record AzAfdOriginListOptions(
[property: CliOption("--origin-group-name")] string OriginGroupName,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;