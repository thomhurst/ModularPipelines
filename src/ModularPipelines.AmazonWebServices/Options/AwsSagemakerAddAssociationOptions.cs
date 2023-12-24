using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "add-association")]
public record AwsSagemakerAddAssociationOptions(
[property: CommandSwitch("--source-arn")] string SourceArn,
[property: CommandSwitch("--destination-arn")] string DestinationArn
) : AwsOptions
{
    [CommandSwitch("--association-type")]
    public string? AssociationType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}