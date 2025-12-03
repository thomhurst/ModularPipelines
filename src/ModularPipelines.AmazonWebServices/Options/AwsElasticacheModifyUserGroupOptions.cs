using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "modify-user-group")]
public record AwsElasticacheModifyUserGroupOptions(
[property: CliOption("--user-group-id")] string UserGroupId
) : AwsOptions
{
    [CliOption("--user-ids-to-add")]
    public string[]? UserIdsToAdd { get; set; }

    [CliOption("--user-ids-to-remove")]
    public string[]? UserIdsToRemove { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}