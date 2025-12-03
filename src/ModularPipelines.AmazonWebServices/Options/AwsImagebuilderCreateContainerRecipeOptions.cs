using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("imagebuilder", "create-container-recipe")]
public record AwsImagebuilderCreateContainerRecipeOptions(
[property: CliOption("--container-type")] string ContainerType,
[property: CliOption("--name")] string Name,
[property: CliOption("--semantic-version")] string SemanticVersion,
[property: CliOption("--components")] string[] Components,
[property: CliOption("--parent-image")] string ParentImage,
[property: CliOption("--target-repository")] string TargetRepository
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--instance-configuration")]
    public string? InstanceConfiguration { get; set; }

    [CliOption("--dockerfile-template-data")]
    public string? DockerfileTemplateData { get; set; }

    [CliOption("--dockerfile-template-uri")]
    public string? DockerfileTemplateUri { get; set; }

    [CliOption("--platform-override")]
    public string? PlatformOverride { get; set; }

    [CliOption("--image-os-version-override")]
    public string? ImageOsVersionOverride { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--working-directory")]
    public string? AwsImagWorkingDirectory { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}