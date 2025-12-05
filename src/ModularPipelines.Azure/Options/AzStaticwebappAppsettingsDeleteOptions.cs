using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("staticwebapp", "appsettings", "delete")]
public record AzStaticwebappAppsettingsDeleteOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--setting-names")] string SettingNames
) : AzOptions
{
    [CliOption("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}