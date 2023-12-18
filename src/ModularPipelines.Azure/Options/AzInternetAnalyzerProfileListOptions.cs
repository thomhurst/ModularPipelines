using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("internet-analyzer", "profile", "list")]
public record AzInternetAnalyzerProfileListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}