using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("deployment", "mg", "create")]
public record AzDeploymentMgCreateOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--management-group-id")] string ManagementGroupId
) : AzOptions
{
    [CliOption("--confirm-with-what-if")]
    public string? ConfirmWithWhatIf { get; set; }

    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-prompt")]
    public bool? NoPrompt { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--proceed-if-no-change")]
    public string? ProceedIfNoChange { get; set; }

    [CliOption("--query-string")]
    public string? QueryString { get; set; }

    [CliOption("--template-file")]
    public string? TemplateFile { get; set; }

    [CliOption("--template-spec")]
    public string? TemplateSpec { get; set; }

    [CliOption("--template-uri")]
    public string? TemplateUri { get; set; }

    [CliOption("--what-if")]
    public string? WhatIf { get; set; }

    [CliOption("--what-if-exclude-change-types")]
    public string? WhatIfExcludeChangeTypes { get; set; }

    [CliOption("--what-if-result-format")]
    public string? WhatIfResultFormat { get; set; }
}