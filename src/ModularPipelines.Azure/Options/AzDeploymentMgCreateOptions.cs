using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "mg", "create")]
public record AzDeploymentMgCreateOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--management-group-id")] string ManagementGroupId
) : AzOptions
{
    [CommandSwitch("--confirm-with-what-if")]
    public string? ConfirmWithWhatIf { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-prompt")]
    public bool? NoPrompt { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

    [CommandSwitch("--proceed-if-no-change")]
    public string? ProceedIfNoChange { get; set; }

    [CommandSwitch("--query-string")]
    public string? QueryString { get; set; }

    [CommandSwitch("--template-file")]
    public string? TemplateFile { get; set; }

    [CommandSwitch("--template-spec")]
    public string? TemplateSpec { get; set; }

    [CommandSwitch("--template-uri")]
    public string? TemplateUri { get; set; }

    [CommandSwitch("--what-if")]
    public string? WhatIf { get; set; }

    [CommandSwitch("--what-if-exclude-change-types")]
    public string? WhatIfExcludeChangeTypes { get; set; }

    [CommandSwitch("--what-if-result-format")]
    public string? WhatIfResultFormat { get; set; }
}