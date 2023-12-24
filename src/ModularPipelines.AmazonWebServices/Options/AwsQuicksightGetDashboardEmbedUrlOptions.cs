using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "get-dashboard-embed-url")]
public record AwsQuicksightGetDashboardEmbedUrlOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--dashboard-id")] string DashboardId,
[property: CommandSwitch("--identity-type")] string IdentityType
) : AwsOptions
{
    [CommandSwitch("--session-lifetime-in-minutes")]
    public long? SessionLifetimeInMinutes { get; set; }

    [CommandSwitch("--user-arn")]
    public string? UserArn { get; set; }

    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [CommandSwitch("--additional-dashboard-ids")]
    public string[]? AdditionalDashboardIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}