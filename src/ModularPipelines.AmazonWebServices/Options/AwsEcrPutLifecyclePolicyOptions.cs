using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr", "put-lifecycle-policy")]
public record AwsEcrPutLifecyclePolicyOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--lifecycle-policy-text")] string LifecyclePolicyText
) : AwsOptions
{
    [CliOption("--registry-id")]
    public string? RegistryId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}