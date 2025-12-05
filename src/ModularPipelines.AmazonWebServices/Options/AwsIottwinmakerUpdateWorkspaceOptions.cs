using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iottwinmaker", "update-workspace")]
public record AwsIottwinmakerUpdateWorkspaceOptions(
[property: CliOption("--workspace-id")] string WorkspaceId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--s3-location")]
    public string? S3Location { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}