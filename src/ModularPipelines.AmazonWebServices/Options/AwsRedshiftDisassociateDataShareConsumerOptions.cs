using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "disassociate-data-share-consumer")]
public record AwsRedshiftDisassociateDataShareConsumerOptions(
[property: CommandSwitch("--data-share-arn")] string DataShareArn
) : AwsOptions
{
    [CommandSwitch("--consumer-arn")]
    public string? ConsumerArn { get; set; }

    [CommandSwitch("--consumer-region")]
    public string? ConsumerRegion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}