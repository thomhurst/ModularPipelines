using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "delete-client-branding")]
public record AwsWorkspacesDeleteClientBrandingOptions(
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--platforms")] string[] Platforms
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}