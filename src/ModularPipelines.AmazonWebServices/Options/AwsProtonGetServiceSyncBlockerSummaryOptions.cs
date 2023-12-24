using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("proton", "get-service-sync-blocker-summary")]
public record AwsProtonGetServiceSyncBlockerSummaryOptions(
[property: CommandSwitch("--service-name")] string ServiceName
) : AwsOptions
{
    [CommandSwitch("--service-instance-name")]
    public string? ServiceInstanceName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}