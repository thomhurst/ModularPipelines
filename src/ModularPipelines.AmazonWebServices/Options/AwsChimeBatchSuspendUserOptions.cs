using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "batch-suspend-user")]
public record AwsChimeBatchSuspendUserOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--user-id-list")] string[] UserIdList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}