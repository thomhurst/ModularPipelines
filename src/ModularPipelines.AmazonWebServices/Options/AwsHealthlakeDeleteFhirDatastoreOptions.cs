using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthlake", "delete-fhir-datastore")]
public record AwsHealthlakeDeleteFhirDatastoreOptions(
[property: CliOption("--datastore-id")] string DatastoreId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}