using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apim", "api", "list")]
public record AzApimApiListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions
{
    [CliOption("--filter-display-name")]
    public string? FilterDisplayName { get; set; }

    [CliOption("--skip")]
    public string? Skip { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}