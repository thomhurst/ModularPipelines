using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cli-translator", "arm", "translate")]
public record AzCliTranslatorArmTranslateOptions(
[property: CommandSwitch("--parameters")] string Parameters,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--template")] string Template
) : AzOptions
{
    [CommandSwitch("--target-subscription")]
    public string? TargetSubscription { get; set; }
}

