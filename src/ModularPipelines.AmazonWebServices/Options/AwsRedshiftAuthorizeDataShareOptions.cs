using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "authorize-data-share")]
public record AwsRedshiftAuthorizeDataShareOptions(
[property: CommandSwitch("--data-share-arn")] string DataShareArn,
[property: CommandSwitch("--consumer-identifier")] string ConsumerIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}