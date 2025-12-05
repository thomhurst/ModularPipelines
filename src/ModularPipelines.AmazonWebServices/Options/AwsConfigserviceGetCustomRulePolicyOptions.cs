using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "get-custom-rule-policy")]
public record AwsConfigserviceGetCustomRulePolicyOptions : AwsOptions
{
    [CliOption("--config-rule-name")]
    public string? ConfigRuleName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}