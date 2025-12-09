using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("repostspace", "deregister-admin")]
public record AwsRepostspaceDeregisterAdminOptions(
[property: CliOption("--admin-id")] string AdminId,
[property: CliOption("--space-id")] string SpaceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}