using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("deployment", "mg", "validate")]
public record AzDeploymentMgValidateOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--management-group-id")] string ManagementGroupId
) : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-prompt")]
    public bool? NoPrompt { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--query-string")]
    public string? QueryString { get; set; }

    [CliOption("--template-file")]
    public string? TemplateFile { get; set; }

    [CliOption("--template-spec")]
    public string? TemplateSpec { get; set; }

    [CliOption("--template-uri")]
    public string? TemplateUri { get; set; }
}