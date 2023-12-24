using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("savingsplans", "delete-queued-savings-plan")]
public record AwsSavingsplansDeleteQueuedSavingsPlanOptions(
[property: CommandSwitch("--savings-plan-id")] string SavingsPlanId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}