using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecr", "get-lifecycle-policy-preview")]
public record AwsEcrGetLifecyclePolicyPreviewOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName
) : AwsOptions
{
    [CommandSwitch("--registry-id")]
    public string? RegistryId { get; set; }

    [CommandSwitch("--image-ids")]
    public string[]? ImageIds { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}