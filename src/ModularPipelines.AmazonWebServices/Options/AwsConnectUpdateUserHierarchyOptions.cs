using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-user-hierarchy")]
public record AwsConnectUpdateUserHierarchyOptions(
[property: CliOption("--user-id")] string UserId,
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--hierarchy-group-id")]
    public string? HierarchyGroupId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}