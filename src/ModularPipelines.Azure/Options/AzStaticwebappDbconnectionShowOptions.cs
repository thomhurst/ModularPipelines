using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("staticwebapp", "dbconnection", "show")]
public record AzStaticwebappDbconnectionShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--detailed")]
    public bool? Detailed { get; set; }

    [CliOption("--environment")]
    public string? Environment { get; set; }
}