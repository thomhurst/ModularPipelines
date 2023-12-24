using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecr-public", "set-repository-policy")]
public record AwsEcrPublicSetRepositoryPolicyOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--policy-text")] string PolicyText
) : AwsOptions
{
    [CommandSwitch("--registry-id")]
    public string? RegistryId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}