using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "update-continuous-backups")]
public record AwsDynamodbUpdateContinuousBackupsOptions(
[property: CommandSwitch("--table-name")] string TableName,
[property: CommandSwitch("--point-in-time-recovery-specification")] string PointInTimeRecoverySpecification
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}