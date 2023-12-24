using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "delete-association")]
public record AwsSagemakerDeleteAssociationOptions(
[property: CommandSwitch("--source-arn")] string SourceArn,
[property: CommandSwitch("--destination-arn")] string DestinationArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}