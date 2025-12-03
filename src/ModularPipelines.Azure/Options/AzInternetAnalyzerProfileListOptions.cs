using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("internet-analyzer", "profile", "list")]
public record AzInternetAnalyzerProfileListOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;