using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediastore-data", "put-object")]
public record AwsMediastoreDataPutObjectOptions(
[property: CommandSwitch("--body")] string Body,
[property: CommandSwitch("--path")] string Path
) : AwsOptions
{
    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--cache-control")]
    public string? CacheControl { get; set; }

    [CommandSwitch("--storage-class")]
    public string? StorageClass { get; set; }

    [CommandSwitch("--upload-availability")]
    public string? UploadAvailability { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}