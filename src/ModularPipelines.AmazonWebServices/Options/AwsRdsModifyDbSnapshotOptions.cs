using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "modify-db-snapshot")]
public record AwsRdsModifyDbSnapshotOptions(
[property: CommandSwitch("--db-snapshot-identifier")] string DbSnapshotIdentifier
) : AwsOptions
{
    [CommandSwitch("--engine-version")]
    public string? EngineVersion { get; set; }

    [CommandSwitch("--option-group-name")]
    public string? OptionGroupName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}