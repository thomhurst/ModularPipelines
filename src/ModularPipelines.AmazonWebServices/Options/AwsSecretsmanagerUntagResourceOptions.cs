using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secretsmanager", "untag-resource")]
public record AwsSecretsmanagerUntagResourceOptions(
[property: CliOption("--secret-id")] string SecretId,
[property: CliOption("--tag-keys")] string[] TagKeys
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}