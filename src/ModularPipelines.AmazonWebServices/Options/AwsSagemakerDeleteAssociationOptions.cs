using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "delete-association")]
public record AwsSagemakerDeleteAssociationOptions(
[property: CliOption("--source-arn")] string SourceArn,
[property: CliOption("--destination-arn")] string DestinationArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}