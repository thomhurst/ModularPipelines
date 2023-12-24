using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "copy-db-parameter-group")]
public record AwsRdsCopyDbParameterGroupOptions(
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