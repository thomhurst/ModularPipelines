using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cli-translator", "arm", "translate")]
public record AzCliTranslatorArmTranslateOptions(
[property: CliOption("--parameters")] string[] Parameters,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--template")] string Template
) : AzOptions
{
    [CliOption("--target-subscription")]
    public string? TargetSubscription { get; set; }
}