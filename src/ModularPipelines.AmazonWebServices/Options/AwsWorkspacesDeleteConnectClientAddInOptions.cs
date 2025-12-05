using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "delete-connect-client-add-in")]
public record AwsWorkspacesDeleteConnectClientAddInOptions(
[property: CliOption("--add-in-id")] string AddInId,
[property: CliOption("--resource-id")] string ResourceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}