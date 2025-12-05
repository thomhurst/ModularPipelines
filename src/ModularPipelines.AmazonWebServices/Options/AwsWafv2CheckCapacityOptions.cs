using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "check-capacity")]
public record AwsWafv2CheckCapacityOptions(
[property: CliOption("--scope")] string Scope,
[property: CliOption("--rules")] string[] Rules
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}