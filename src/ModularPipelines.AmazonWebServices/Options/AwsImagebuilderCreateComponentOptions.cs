using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("imagebuilder", "create-component")]
public record AwsImagebuilderCreateComponentOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--semantic-version")] string SemanticVersion,
[property: CliOption("--platform")] string Platform
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--change-description")]
    public string? ChangeDescription { get; set; }

    [CliOption("--supported-os-versions")]
    public string[]? SupportedOsVersions { get; set; }

    [CliOption("--data")]
    public string? Data { get; set; }

    [CliOption("--uri")]
    public string? Uri { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}