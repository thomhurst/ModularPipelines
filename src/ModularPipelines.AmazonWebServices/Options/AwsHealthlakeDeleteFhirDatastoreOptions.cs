using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthlake", "delete-fhir-datastore")]
public record AwsHealthlakeDeleteFhirDatastoreOptions(
[property: CommandSwitch("--datastore-id")] string DatastoreId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}