using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("savingsplans", "describe-savings-plans")]
public record AwsSavingsplansDescribeSavingsPlansOptions : AwsOptions
{
    [CommandSwitch("--savings-plan-arns")]
    public string[]? SavingsPlanArns { get; set; }

    [CommandSwitch("--savings-plan-ids")]
    public string[]? SavingsPlanIds { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--states")]
    public string[]? States { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}