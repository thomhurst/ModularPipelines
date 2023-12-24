using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "upgrade-profile-version")]
public record AwsWellarchitectedUpgradeProfileVersionOptions(
[property: CommandSwitch("--workload-id")] string WorkloadId,
[property: CommandSwitch("--profile-arn")] string ProfileArn
) : AwsOptions
{
    [CommandSwitch("--milestone-name")]
    public string? MilestoneName { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}