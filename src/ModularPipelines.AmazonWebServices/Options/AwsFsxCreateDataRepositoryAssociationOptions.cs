using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fsx", "create-data-repository-association")]
public record AwsFsxCreateDataRepositoryAssociationOptions(
[property: CliOption("--file-system-id")] string FileSystemId,
[property: CliOption("--data-repository-path")] string DataRepositoryPath
) : AwsOptions
{
    [CliOption("--file-system-path")]
    public string? FileSystemPath { get; set; }

    [CliOption("--imported-file-chunk-size")]
    public int? ImportedFileChunkSize { get; set; }

    [CliOption("--s3")]
    public string? S3 { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}