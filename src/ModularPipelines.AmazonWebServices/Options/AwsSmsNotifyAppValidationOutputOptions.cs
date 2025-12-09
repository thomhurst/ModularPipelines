using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sms", "notify-app-validation-output")]
public record AwsSmsNotifyAppValidationOutputOptions(
[property: CliOption("--app-id")] string AppId
) : AwsOptions
{
    [CliOption("--notification-context")]
    public string? NotificationContext { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}