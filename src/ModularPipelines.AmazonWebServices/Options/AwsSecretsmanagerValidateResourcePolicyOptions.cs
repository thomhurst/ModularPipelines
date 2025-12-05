using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secretsmanager", "validate-resource-policy")]
public record AwsSecretsmanagerValidateResourcePolicyOptions(
[property: CliOption("--resource-policy")] string ResourcePolicy
) : AwsOptions
{
    [CliOption("--secret-id")]
    public string? SecretId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}