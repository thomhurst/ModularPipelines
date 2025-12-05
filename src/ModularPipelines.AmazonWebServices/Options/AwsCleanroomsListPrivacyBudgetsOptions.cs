using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "list-privacy-budgets")]
public record AwsCleanroomsListPrivacyBudgetsOptions(
[property: CliOption("--membership-identifier")] string MembershipIdentifier,
[property: CliOption("--privacy-budget-type")] string PrivacyBudgetType
) : AwsOptions
{
    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}