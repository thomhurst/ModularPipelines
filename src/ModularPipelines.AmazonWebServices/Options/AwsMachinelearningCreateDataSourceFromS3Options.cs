using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("machinelearning", "create-data-source-from-s3")]
public record AwsMachinelearningCreateDataSourceFromS3Options(
[property: CommandSwitch("--data-source-id")] string DataSourceId,
[property: CommandSwitch("--data-spec")] string DataSpec
) : AwsOptions
{
    [CommandSwitch("--data-source-name")]
    public string? DataSourceName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}