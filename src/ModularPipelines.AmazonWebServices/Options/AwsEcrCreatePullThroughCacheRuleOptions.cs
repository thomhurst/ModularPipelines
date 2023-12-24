using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecr", "create-pull-through-cache-rule")]
public record AwsEcrCreatePullThroughCacheRuleOptions(
[property: CommandSwitch("--ecr-repository-prefix")] string EcrRepositoryPrefix,
[property: CommandSwitch("--upstream-registry-url")] string UpstreamRegistryUrl,
[property: CommandSwitch("--upstream-registry")] string UpstreamRegistry
) : AwsOptions
{
    [CommandSwitch("--registry-id")]
    public string? RegistryId { get; set; }

    [CommandSwitch("--credential-arn")]
    public string? CredentialArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}