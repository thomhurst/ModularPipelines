using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("imagebuilder", "create-image-recipe")]
public record AwsImagebuilderCreateImageRecipeOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--semantic-version")] string SemanticVersion,
[property: CliOption("--components")] string[] Components,
[property: CliOption("--parent-image")] string ParentImage
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--block-device-mappings")]
    public string[]? BlockDeviceMappings { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--working-directory")]
    public string? AwsImagWorkingDirectory { get; set; }

    [CliOption("--additional-instance-configuration")]
    public string? AdditionalInstanceConfiguration { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}