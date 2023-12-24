using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecr-public", "create-repository")]
public record AwsEcrPublicCreateRepositoryOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName
) : AwsOptions
{
    [CommandSwitch("--catalog-data")]
    public string? CatalogData { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}