using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "update-public-sharing-settings")]
public record AwsQuicksightUpdatePublicSharingSettingsOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}