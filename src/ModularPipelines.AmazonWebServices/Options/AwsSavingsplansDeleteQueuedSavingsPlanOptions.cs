using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("savingsplans", "delete-queued-savings-plan")]
public record AwsSavingsplansDeleteQueuedSavingsPlanOptions(
[property: CliOption("--savings-plan-id")] string SavingsPlanId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}