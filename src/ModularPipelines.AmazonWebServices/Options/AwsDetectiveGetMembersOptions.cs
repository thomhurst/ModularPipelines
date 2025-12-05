using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("detective", "get-members")]
public record AwsDetectiveGetMembersOptions(
[property: CliOption("--graph-arn")] string GraphArn,
[property: CliOption("--account-ids")] string[] AccountIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}