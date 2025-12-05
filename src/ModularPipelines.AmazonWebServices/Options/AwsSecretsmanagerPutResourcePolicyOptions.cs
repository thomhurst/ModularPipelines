using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secretsmanager", "put-resource-policy")]
public record AwsSecretsmanagerPutResourcePolicyOptions(
[property: CliOption("--secret-id")] string SecretId,
[property: CliOption("--resource-policy")] string ResourcePolicy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}