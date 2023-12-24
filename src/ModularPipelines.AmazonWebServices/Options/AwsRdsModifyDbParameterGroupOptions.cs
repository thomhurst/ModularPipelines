using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "modify-db-parameter-group")]
public record AwsRdsModifyDbParameterGroupOptions(
[property: CommandSwitch("--db-parameter-group-name")] string DbParameterGroupName,
[property: CommandSwitch("--parameters")] string[] Parameters
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}