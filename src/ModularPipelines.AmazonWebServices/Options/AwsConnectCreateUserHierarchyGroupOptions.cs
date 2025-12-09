using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "create-user-hierarchy-group")]
public record AwsConnectCreateUserHierarchyGroupOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--parent-group-id")]
    public string? ParentGroupId { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}