using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("omics", "create-multipart-read-set-upload")]
public record AwsOmicsCreateMultipartReadSetUploadOptions(
[property: CommandSwitch("--sequence-store-id")] string SequenceStoreId,
[property: CommandSwitch("--source-file-type")] string SourceFileType,
[property: CommandSwitch("--subject-id")] string SubjectId,
[property: CommandSwitch("--sample-id")] string SampleId,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generated-from")]
    public string? GeneratedFrom { get; set; }

    [CommandSwitch("--reference-arn")]
    public string? ReferenceArn { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}