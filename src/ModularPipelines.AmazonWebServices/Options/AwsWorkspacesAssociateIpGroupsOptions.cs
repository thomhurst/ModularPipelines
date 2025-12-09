using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "associate-ip-groups")]
public record AwsWorkspacesAssociateIpGroupsOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--group-ids")] string[] GroupIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}