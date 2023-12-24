using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "delete-data-set-refresh-properties")]
public record AwsQuicksightDeleteDataSetRefreshPropertiesOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--data-set-id")] string DataSetId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}