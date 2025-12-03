using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fsx", "update-data-repository-association")]
public record AwsFsxUpdateDataRepositoryAssociationOptions(
[property: CliOption("--association-id")] string AssociationId
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--imported-file-chunk-size")]
    public int? ImportedFileChunkSize { get; set; }

    [CliOption("--s3")]
    public string? S3 { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}