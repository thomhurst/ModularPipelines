using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mgn", "disassociate-applications")]
public record AwsMgnDisassociateApplicationsOptions(
[property: CliOption("--application-ids")] string[] ApplicationIds,
[property: CliOption("--wave-id")] string WaveId
) : AwsOptions
{
    [CliOption("--account-id")]
    public string? AccountId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}