using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "deregister-job-definition")]
public record AwsBatchDeregisterJobDefinitionOptions(
[property: CliOption("--job-definition")] string JobDefinition
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}