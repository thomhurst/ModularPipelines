using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mgh", "notify-application-state")]
public record AwsMghNotifyApplicationStateOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--status")] string Status
) : AwsOptions
{
    [CliOption("--update-date-time")]
    public long? UpdateDateTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}