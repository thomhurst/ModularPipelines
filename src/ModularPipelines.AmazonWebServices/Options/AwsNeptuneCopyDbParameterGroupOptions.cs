using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptune", "copy-db-parameter-group")]
public record AwsNeptuneCopyDbParameterGroupOptions(
[property: CommandSwitch("--source-db-parameter-group-identifier")] string SourceDbParameterGroupIdentifier,
[property: CommandSwitch("--target-db-parameter-group-identifier")] string TargetDbParameterGroupIdentifier,
[property: CommandSwitch("--target-db-parameter-group-description")] string TargetDbParameterGroupDescription
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}