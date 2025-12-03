using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediastore-data", "put-object")]
public record AwsMediastoreDataPutObjectOptions(
[property: CliOption("--body")] string Body,
[property: CliOption("--path")] string Path
) : AwsOptions
{
    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--cache-control")]
    public string? CacheControl { get; set; }

    [CliOption("--storage-class")]
    public string? StorageClass { get; set; }

    [CliOption("--upload-availability")]
    public string? UploadAvailability { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}