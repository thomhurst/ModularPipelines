using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "batch-update-user")]
public record AwsChimeBatchUpdateUserOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--update-user-request-items")] string[] UpdateUserRequestItems
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}