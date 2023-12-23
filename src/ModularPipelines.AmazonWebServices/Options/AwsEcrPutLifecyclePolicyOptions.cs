using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecr", "put-lifecycle-policy")]
public record AwsEcrPutLifecyclePolicyOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--lifecycle-policy-text")] string LifecyclePolicyText
) : AwsOptions
{
    [CommandSwitch("--registry-id")]
    public string? RegistryId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}