using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecr", "batch-get-image")]
public record AwsEcrBatchGetImageOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--image-ids")] string[] ImageIds
) : AwsOptions
{
    [CommandSwitch("--registry-id")]
    public string? RegistryId { get; set; }

    [CommandSwitch("--accepted-media-types")]
    public string[]? AcceptedMediaTypes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}