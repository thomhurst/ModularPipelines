using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("machinelearning", "create-data-source-from-redshift")]
public record AwsMachinelearningCreateDataSourceFromRedshiftOptions(
[property: CommandSwitch("--data-source-id")] string DataSourceId,
[property: CommandSwitch("--data-spec")] string DataSpec,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--data-source-name")]
    public string? DataSourceName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}