using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "create-multipart-read-set-upload")]
public record AwsOmicsCreateMultipartReadSetUploadOptions(
[property: CliOption("--sequence-store-id")] string SequenceStoreId,
[property: CliOption("--source-file-type")] string SourceFileType,
[property: CliOption("--subject-id")] string SubjectId,
[property: CliOption("--sample-id")] string SampleId,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generated-from")]
    public string? GeneratedFrom { get; set; }

    [CliOption("--reference-arn")]
    public string? ReferenceArn { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}