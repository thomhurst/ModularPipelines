using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "get-dashboard-embed-url")]
public record AwsQuicksightGetDashboardEmbedUrlOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--dashboard-id")] string DashboardId,
[property: CliOption("--identity-type")] string IdentityType
) : AwsOptions
{
    [CliOption("--session-lifetime-in-minutes")]
    public long? SessionLifetimeInMinutes { get; set; }

    [CliOption("--user-arn")]
    public string? UserArn { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--additional-dashboard-ids")]
    public string[]? AdditionalDashboardIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}