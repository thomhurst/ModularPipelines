using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("staticwebapp", "appsettings", "list")]
public record AzStaticwebappAppsettingsListOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}