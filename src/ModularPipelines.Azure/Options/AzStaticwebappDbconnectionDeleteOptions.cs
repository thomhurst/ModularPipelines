using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("staticwebapp", "dbconnection", "delete")]
public record AzStaticwebappDbconnectionDeleteOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--environment")]
    public string? Environment { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}