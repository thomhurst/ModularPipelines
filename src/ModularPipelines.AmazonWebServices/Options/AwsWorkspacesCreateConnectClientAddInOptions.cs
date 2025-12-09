using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "create-connect-client-add-in")]
public record AwsWorkspacesCreateConnectClientAddInOptions(
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--name")] string Name,
[property: CliOption("--url")] string Url
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}