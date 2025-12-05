using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "create-members")]
public record AwsGuarddutyCreateMembersOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--account-details")] string[] AccountDetails
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}