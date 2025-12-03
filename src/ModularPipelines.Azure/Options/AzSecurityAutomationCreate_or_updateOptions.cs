using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "automation", "create_or_update")]
public record AzSecurityAutomationCreate_or_updateOptions(
[property: CliOption("--actions")] string Actions,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--scopes")] string Scopes,
[property: CliOption("--sources")] string Sources
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--isEnabled")]
    public string? IsEnabled { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}