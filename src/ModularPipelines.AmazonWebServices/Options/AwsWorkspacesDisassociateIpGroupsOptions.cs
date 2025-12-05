using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "disassociate-ip-groups")]
public record AwsWorkspacesDisassociateIpGroupsOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--group-ids")] string[] GroupIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}