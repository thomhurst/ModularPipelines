using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stream-analytics", "function", "delete")]
public record AzStreamAnalyticsFunctionDeleteOptions(
[property: CommandSwitch("--function-name")] string FunctionName,
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}