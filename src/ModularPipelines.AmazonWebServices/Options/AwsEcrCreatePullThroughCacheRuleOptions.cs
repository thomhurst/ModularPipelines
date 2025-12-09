using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr", "create-pull-through-cache-rule")]
public record AwsEcrCreatePullThroughCacheRuleOptions(
[property: CliOption("--ecr-repository-prefix")] string EcrRepositoryPrefix,
[property: CliOption("--upstream-registry-url")] string UpstreamRegistryUrl,
[property: CliOption("--upstream-registry")] string UpstreamRegistry
) : AwsOptions
{
    [CliOption("--registry-id")]
    public string? RegistryId { get; set; }

    [CliOption("--credential-arn")]
    public string? CredentialArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}