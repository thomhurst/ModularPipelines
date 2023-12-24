using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptune", "create-db-parameter-group")]
public record AwsNeptuneCreateDbParameterGroupOptions(
[property: CommandSwitch("--db-parameter-group-name")] string DbParameterGroupName,
[property: CommandSwitch("--db-parameter-group-family")] string DbParameterGroupFamily,
[property: CommandSwitch("--description")] string Description
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}