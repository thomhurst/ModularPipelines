using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecr", "complete-layer-upload")]
public record AwsEcrCompleteLayerUploadOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--upload-id")] string UploadId,
[property: CommandSwitch("--layer-digests")] string[] LayerDigests
) : AwsOptions
{
    [CommandSwitch("--registry-id")]
    public string? RegistryId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}