using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "group", "validate")]
public record AzDeploymentGroupValidateOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-prompt")]
    public bool? NoPrompt { get; set; }

    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

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
}