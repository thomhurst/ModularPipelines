using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "update-account-settings")]
public record AwsQuicksightUpdateAccountSettingsOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--default-namespace")] string DefaultNamespace
) : AwsOptions
{
    [CliOption("--notification-email")]
    public string? NotificationEmail { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}