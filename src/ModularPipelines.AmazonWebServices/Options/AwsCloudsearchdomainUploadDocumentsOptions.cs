using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudsearchdomain", "upload-documents")]
public record AwsCloudsearchdomainUploadDocumentsOptions(
[property: CliOption("--documents")] string Documents,
[property: CliOption("--content-type")] string ContentType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}