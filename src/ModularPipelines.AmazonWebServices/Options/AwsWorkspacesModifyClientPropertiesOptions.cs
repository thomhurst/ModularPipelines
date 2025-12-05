using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "modify-client-properties")]
public record AwsWorkspacesModifyClientPropertiesOptions(
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--client-properties")] string ClientProperties
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}