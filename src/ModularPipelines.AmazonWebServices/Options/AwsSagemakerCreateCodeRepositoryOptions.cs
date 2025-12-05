using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-code-repository")]
public record AwsSagemakerCreateCodeRepositoryOptions(
[property: CliOption("--code-repository-name")] string CodeRepositoryName,
[property: CliOption("--git-config")] string GitConfig
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}