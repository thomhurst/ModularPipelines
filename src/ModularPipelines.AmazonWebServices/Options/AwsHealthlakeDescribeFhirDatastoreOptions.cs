using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthlake", "describe-fhir-datastore")]
public record AwsHealthlakeDescribeFhirDatastoreOptions(
[property: CommandSwitch("--datastore-id")] string DatastoreId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}