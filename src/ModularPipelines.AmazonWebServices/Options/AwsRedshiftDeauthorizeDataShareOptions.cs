using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "deauthorize-data-share")]
public record AwsRedshiftDeauthorizeDataShareOptions(
[property: CommandSwitch("--data-share-arn")] string DataShareArn,
[property: CommandSwitch("--consumer-identifier")] string ConsumerIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}