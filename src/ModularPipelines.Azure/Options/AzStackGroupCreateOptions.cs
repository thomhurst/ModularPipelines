using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("stack", "group", "create")]
public record AzStackGroupCreateOptions(
[property: CliOption("--deny-settings-mode")] string DenySettingsMode,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--cs")]
    public bool? Cs { get; set; }

    [CliFlag("--delete-all")]
    public bool? DeleteAll { get; set; }

    [CliFlag("--delete-resource-groups")]
    public bool? DeleteResourceGroups { get; set; }

    [CliFlag("--delete-resources")]
    public bool? DeleteResources { get; set; }

    [CliOption("--deny-settings-excluded-actions")]
    public string? DenySettingsExcludedActions { get; set; }

    [CliOption("--deny-settings-excluded-principals")]
    public string? DenySettingsExcludedPrincipals { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--query-string")]
    public string? QueryString { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--template-file")]
    public string? TemplateFile { get; set; }

    [CliOption("--template-spec")]
    public string? TemplateSpec { get; set; }

    [CliOption("--template-uri")]
    public string? TemplateUri { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}