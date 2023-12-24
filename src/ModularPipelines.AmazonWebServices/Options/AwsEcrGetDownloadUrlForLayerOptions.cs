using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecr", "get-download-url-for-layer")]
public record AwsEcrGetDownloadUrlForLayerOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--layer-digest")] string LayerDigest
) : AwsOptions
{
    [CommandSwitch("--registry-id")]
    public string? RegistryId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}