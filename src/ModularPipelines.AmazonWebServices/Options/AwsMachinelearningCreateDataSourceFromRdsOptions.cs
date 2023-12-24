using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("machinelearning", "create-data-source-from-rds")]
public record AwsMachinelearningCreateDataSourceFromRdsOptions(
[property: CommandSwitch("--data-source-id")] string DataSourceId,
[property: CommandSwitch("--rds-data")] string RdsData,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--data-source-name")]
    public string? DataSourceName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}