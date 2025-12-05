using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr-public", "set-repository-policy")]
public record AwsEcrPublicSetRepositoryPolicyOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--policy-text")] string PolicyText
) : AwsOptions
{
    [CliOption("--registry-id")]
    public string? RegistryId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}