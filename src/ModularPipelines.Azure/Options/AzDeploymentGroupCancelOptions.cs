using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "group", "cancel")]
public record AzDeploymentGroupCancelOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--aux-subs")]
    public string? AuxSubs { get; set; }

    [CommandSwitch("--aux-tenants")]
    public string? AuxTenants { get; set; }

    [CommandSwitch("--confirm-with-what-if")]
    public string? ConfirmWithWhatIf { get; set; }

    [CommandSwitch("--handle-extended-json-format")]
    public string? HandleExtendedJsonFormat { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [BooleanCommandSwitch("--no-prompt")]
    public bool? NoPrompt { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--parameters")]
    public string? Parameters { get; set; }

    [CommandSwitch("--proceed-if-no-change")]
    public string? ProceedIfNoChange { get; set; }

    [CommandSwitch("--query-string")]
    public string? QueryString { get; set; }

    [CommandSwitch("--rollback-on-error")]
    public string? RollbackOnError { get; set; }

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

