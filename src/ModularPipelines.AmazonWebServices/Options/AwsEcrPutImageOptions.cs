using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecr", "put-image")]
public record AwsEcrPutImageOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--image-manifest")] string ImageManifest
) : AwsOptions
{
    [CommandSwitch("--registry-id")]
    public string? RegistryId { get; set; }

    [CommandSwitch("--image-manifest-media-type")]
    public string? ImageManifestMediaType { get; set; }

    [CommandSwitch("--image-tag")]
    public string? ImageTag { get; set; }

    [CommandSwitch("--image-digest")]
    public string? ImageDigest { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}