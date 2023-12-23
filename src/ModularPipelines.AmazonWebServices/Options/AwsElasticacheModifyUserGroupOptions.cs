using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "modify-user-group")]
public record AwsElasticacheModifyUserGroupOptions(
[property: CommandSwitch("--user-group-id")] string UserGroupId
) : AwsOptions
{
    [CommandSwitch("--user-ids-to-add")]
    public string[]? UserIdsToAdd { get; set; }

    [CommandSwitch("--user-ids-to-remove")]
    public string[]? UserIdsToRemove { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}