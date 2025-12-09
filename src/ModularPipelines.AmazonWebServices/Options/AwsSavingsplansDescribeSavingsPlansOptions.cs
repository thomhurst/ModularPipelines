using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("savingsplans", "describe-savings-plans")]
public record AwsSavingsplansDescribeSavingsPlansOptions : AwsOptions
{
    [CliOption("--savings-plan-arns")]
    public string[]? SavingsPlanArns { get; set; }

    [CliOption("--savings-plan-ids")]
    public string[]? SavingsPlanIds { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--states")]
    public string[]? States { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}