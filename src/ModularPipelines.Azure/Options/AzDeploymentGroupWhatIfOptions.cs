using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment", "group", "what-if")]
public record AzDeploymentGroupWhatIfOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aux-tenants")]
    public string? AuxTenants { get; set; }

    [CliOption("--exclude-change-types")]
    public string? ExcludeChangeTypes { get; set; }

    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-pretty-print")]
    public bool? NoPrettyPrint { get; set; }

    [CliFlag("--no-prompt")]
    public bool? NoPrompt { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--query-string")]
    public string? QueryString { get; set; }

    [CliOption("--result-format")]
    public string? ResultFormat { get; set; }

    [CliOption("--template-file")]
    public string? TemplateFile { get; set; }

    [CliOption("--template-spec")]
    public string? TemplateSpec { get; set; }

    [CliOption("--template-uri")]
    public string? TemplateUri { get; set; }
}