using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecr", "start-lifecycle-policy-preview")]
public record AwsEcrStartLifecyclePolicyPreviewOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName
) : AwsOptions
{
    [CommandSwitch("--registry-id")]
    public string? RegistryId { get; set; }

    [CommandSwitch("--lifecycle-policy-text")]
    public string? LifecyclePolicyText { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}