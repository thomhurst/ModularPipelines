using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "update-connect-client-add-in")]
public record AwsWorkspacesUpdateConnectClientAddInOptions(
[property: CliOption("--add-in-id")] string AddInId,
[property: CliOption("--resource-id")] string ResourceId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--url")]
    public string? Url { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}