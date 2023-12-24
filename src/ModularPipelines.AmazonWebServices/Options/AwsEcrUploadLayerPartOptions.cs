using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecr", "upload-layer-part")]
public record AwsEcrUploadLayerPartOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--upload-id")] string UploadId,
[property: CommandSwitch("--part-first-byte")] long PartFirstByte,
[property: CommandSwitch("--part-last-byte")] long PartLastByte,
[property: CommandSwitch("--layer-part-blob")] string LayerPartBlob
) : AwsOptions
{
    [CommandSwitch("--registry-id")]
    public string? RegistryId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}