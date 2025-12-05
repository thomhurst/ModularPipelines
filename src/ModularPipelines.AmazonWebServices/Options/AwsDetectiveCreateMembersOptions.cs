using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("detective", "create-members")]
public record AwsDetectiveCreateMembersOptions(
[property: CliOption("--graph-arn")] string GraphArn,
[property: CliOption("--accounts")] string[] Accounts
) : AwsOptions
{
    [CliOption("--message")]
    public string? Message { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}