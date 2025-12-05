using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog-appregistry", "get-application")]
public record AwsServicecatalogAppregistryGetApplicationOptions(
[property: CliOption("--application")] string Application
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}