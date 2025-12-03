using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apim", "nv", "delete")]
public record AzApimNvDeleteOptions(
[property: CliOption("--named-value-id")] string NamedValueId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}