using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dynamodb", "update-time-to-live")]
public record AwsDynamodbUpdateTimeToLiveOptions(
[property: CommandSwitch("--table-name")] string TableName,
[property: CommandSwitch("--time-to-live-specification")] string TimeToLiveSpecification
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}