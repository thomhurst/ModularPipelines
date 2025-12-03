using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "update-key-description")]
public record AwsKmsUpdateKeyDescriptionOptions(
[property: CliOption("--key-id")] string KeyId,
[property: CliOption("--description")] string Description
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}