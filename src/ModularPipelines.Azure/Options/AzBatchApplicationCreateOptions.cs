using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "application", "create")]
public record AzBatchApplicationCreateOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }
}