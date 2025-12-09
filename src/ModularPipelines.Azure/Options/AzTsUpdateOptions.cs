using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ts", "update")]
public record AzTsUpdateOptions : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--template-file")]
    public string? TemplateFile { get; set; }

    [CliOption("--template-spec")]
    public string? TemplateSpec { get; set; }

    [CliOption("--ui-form-definition")]
    public string? UiFormDefinition { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliOption("--version-description")]
    public string? VersionDescription { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}