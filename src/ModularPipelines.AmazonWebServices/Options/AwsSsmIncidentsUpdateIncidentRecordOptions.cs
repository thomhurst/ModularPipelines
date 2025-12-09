using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-incidents", "update-incident-record")]
public record AwsSsmIncidentsUpdateIncidentRecordOptions(
[property: CliOption("--arn")] string Arn
) : AwsOptions
{
    [CliOption("--chat-channel")]
    public string? ChatChannel { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--impact")]
    public int? Impact { get; set; }

    [CliOption("--notification-targets")]
    public string[]? NotificationTargets { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--summary")]
    public string? Summary { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}