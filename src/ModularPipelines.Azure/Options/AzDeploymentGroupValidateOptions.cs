using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment", "group", "validate")]
public record AzDeploymentGroupValidateOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-prompt")]
    public bool? NoPrompt { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--query-string")]
    public string? QueryString { get; set; }

    [CliOption("--rollback-on-error")]
    public string? RollbackOnError { get; set; }

    [CliOption("--template-file")]
    public string? TemplateFile { get; set; }

    [CliOption("--template-spec")]
    public string? TemplateSpec { get; set; }

    [CliOption("--template-uri")]
    public string? TemplateUri { get; set; }
}