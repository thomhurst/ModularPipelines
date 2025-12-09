using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-code-repository")]
public record AwsSagemakerUpdateCodeRepositoryOptions(
[property: CliOption("--code-repository-name")] string CodeRepositoryName
) : AwsOptions
{
    [CliOption("--git-config")]
    public string? GitConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}