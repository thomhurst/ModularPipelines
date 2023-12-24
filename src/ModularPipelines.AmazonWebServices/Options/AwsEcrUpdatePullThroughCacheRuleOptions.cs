using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecr", "update-pull-through-cache-rule")]
public record AwsEcrUpdatePullThroughCacheRuleOptions(
[property: CommandSwitch("--ecr-repository-prefix")] string EcrRepositoryPrefix,
[property: CommandSwitch("--credential-arn")] string CredentialArn
) : AwsOptions
{
    [CommandSwitch("--registry-id")]
    public string? RegistryId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}