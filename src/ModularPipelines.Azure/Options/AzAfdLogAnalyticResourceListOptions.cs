using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("afd", "log-analytic", "resource", "list")]
public record AzAfdLogAnalyticResourceListOptions(
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;