using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-workteam")]
public record AwsSagemakerCreateWorkteamOptions(
[property: CliOption("--workteam-name")] string WorkteamName,
[property: CliOption("--member-definitions")] string[] MemberDefinitions,
[property: CliOption("--description")] string Description
) : AwsOptions
{
    [CliOption("--workforce-name")]
    public string? WorkforceName { get; set; }

    [CliOption("--notification-configuration")]
    public string? NotificationConfiguration { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}