using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("stream-analytics", "function", "create")]
public record AzStreamAnalyticsFunctionCreateOptions(
[property: CliOption("--function-name")] string FunctionName,
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--properties")]
    public string? Properties { get; set; }
}