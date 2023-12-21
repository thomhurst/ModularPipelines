using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("image", "builder", "validator", "add")]
public record AzImageBuilderValidatorAddOptions : AzOptions
{
    [BooleanCommandSwitch("--continue-distribute-on-failure")]
    public bool? ContinueDistributeOnFailure { get; set; }

    [CommandSwitch("--defer")]
    public string? Defer { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--source-validation-only")]
    public bool? SourceValidationOnly { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}