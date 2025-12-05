using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-workteam")]
public record AwsSagemakerUpdateWorkteamOptions(
[property: CliOption("--workteam-name")] string WorkteamName
) : AwsOptions
{
    [CliOption("--member-definitions")]
    public string[]? MemberDefinitions { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--notification-configuration")]
    public string? NotificationConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}