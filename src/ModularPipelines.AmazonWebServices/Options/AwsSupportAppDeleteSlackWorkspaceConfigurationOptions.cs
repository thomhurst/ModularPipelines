using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("support-app", "delete-slack-workspace-configuration")]
public record AwsSupportAppDeleteSlackWorkspaceConfigurationOptions(
[property: CliOption("--team-id")] string TeamId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}