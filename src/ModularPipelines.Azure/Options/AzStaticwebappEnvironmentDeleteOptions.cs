using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("staticwebapp", "environment", "delete")]
public record AzStaticwebappEnvironmentDeleteOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}