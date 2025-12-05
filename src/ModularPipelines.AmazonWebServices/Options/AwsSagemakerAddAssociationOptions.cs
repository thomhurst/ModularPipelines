using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "add-association")]
public record AwsSagemakerAddAssociationOptions(
[property: CliOption("--source-arn")] string SourceArn,
[property: CliOption("--destination-arn")] string DestinationArn
) : AwsOptions
{
    [CliOption("--association-type")]
    public string? AssociationType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}