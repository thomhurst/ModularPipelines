using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "create-privacy-budget-template")]
public record AwsCleanroomsCreatePrivacyBudgetTemplateOptions(
[property: CliOption("--membership-identifier")] string MembershipIdentifier,
[property: CliOption("--auto-refresh")] string AutoRefresh,
[property: CliOption("--privacy-budget-type")] string PrivacyBudgetType,
[property: CliOption("--parameters")] string Parameters
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}