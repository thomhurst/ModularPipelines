using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecr", "put-image-tag-mutability")]
public record AwsEcrPutImageTagMutabilityOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--image-tag-mutability")] string ImageTagMutability
) : AwsOptions
{
    [CommandSwitch("--registry-id")]
    public string? RegistryId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}