using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "update-repository-description")]
public record AwsCodecommitUpdateRepositoryDescriptionOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName
) : AwsOptions
{
    [CommandSwitch("--repository-description")]
    public string? RepositoryDescription { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}