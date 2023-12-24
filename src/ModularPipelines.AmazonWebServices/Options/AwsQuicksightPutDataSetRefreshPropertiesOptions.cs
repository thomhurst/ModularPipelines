using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "put-data-set-refresh-properties")]
public record AwsQuicksightPutDataSetRefreshPropertiesOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--data-set-id")] string DataSetId,
[property: CommandSwitch("--data-set-refresh-properties")] string DataSetRefreshProperties
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}