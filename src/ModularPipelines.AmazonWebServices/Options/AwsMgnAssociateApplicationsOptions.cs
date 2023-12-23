using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgn", "associate-applications")]
public record AwsMgnAssociateApplicationsOptions(
[property: CommandSwitch("--application-ids")] string[] ApplicationIds,
[property: CommandSwitch("--wave-id")] string WaveId
) : AwsOptions
{
    [CommandSwitch("--account-id")]
    public string? AccountId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}