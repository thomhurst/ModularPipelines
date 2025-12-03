using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "get-object-information")]
public record AwsClouddirectoryGetObjectInformationOptions(
[property: CliOption("--directory-arn")] string DirectoryArn,
[property: CliOption("--object-reference")] string ObjectReference
) : AwsOptions
{
    [CliOption("--consistency-level")]
    public string? ConsistencyLevel { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}