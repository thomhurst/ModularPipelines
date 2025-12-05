using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr", "update-pull-through-cache-rule")]
public record AwsEcrUpdatePullThroughCacheRuleOptions(
[property: CliOption("--ecr-repository-prefix")] string EcrRepositoryPrefix,
[property: CliOption("--credential-arn")] string CredentialArn
) : AwsOptions
{
    [CliOption("--registry-id")]
    public string? RegistryId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}