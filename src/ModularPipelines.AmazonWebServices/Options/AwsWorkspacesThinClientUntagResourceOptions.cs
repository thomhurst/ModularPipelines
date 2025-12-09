using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-thin-client", "untag-resource")]
public record AwsWorkspacesThinClientUntagResourceOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--tag-keys")] string[] TagKeys
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}