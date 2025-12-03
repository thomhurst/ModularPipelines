using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ts", "create")]
public record AzTsCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--template-file")]
    public string? TemplateFile { get; set; }

    [CliOption("--ui-form-definition")]
    public string? UiFormDefinition { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliOption("--version-description")]
    public string? VersionDescription { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}