using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "trigger-run", "rerun")]
public record AzDatafactoryTriggerRunRerunOptions : AzOptions
{
    [CommandSwitch("--factory-name")]
    public string? FactoryName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--run-id")]
    public string? RunId { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--trigger-name")]
    public string? TriggerName { get; set; }
}