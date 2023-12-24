using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "create-user-group")]
public record AwsElasticacheCreateUserGroupOptions(
[property: CommandSwitch("--user-group-id")] string UserGroupId,
[property: CommandSwitch("--engine")] string Engine
) : AwsOptions
{
    [CommandSwitch("--user-ids")]
    public string[]? UserIds { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}