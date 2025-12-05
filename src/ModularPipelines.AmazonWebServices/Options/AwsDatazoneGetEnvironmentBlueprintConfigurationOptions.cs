using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "get-environment-blueprint-configuration")]
public record AwsDatazoneGetEnvironmentBlueprintConfigurationOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--environment-blueprint-identifier")] string EnvironmentBlueprintIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}