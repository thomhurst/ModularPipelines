using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("image", "builder", "validator", "add")]
public record AzImageBuilderValidatorAddOptions : AzOptions
{
    [CliFlag("--continue-distribute-on-failure")]
    public bool? ContinueDistributeOnFailure { get; set; }

    [CliOption("--defer")]
    public string? Defer { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--source-validation-only")]
    public bool? SourceValidationOnly { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}