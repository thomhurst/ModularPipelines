using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "update-registry")]
public record AwsGlueUpdateRegistryOptions(
[property: CliOption("--registry-id")] string RegistryId,
[property: CliOption("--description")] string Description
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}