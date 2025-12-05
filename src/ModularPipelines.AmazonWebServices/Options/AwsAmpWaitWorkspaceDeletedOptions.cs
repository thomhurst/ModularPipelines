using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amp", "wait", "workspace-deleted")]
public record AwsAmpWaitWorkspaceDeletedOptions(
[property: CliOption("--workspace-id")] string WorkspaceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}