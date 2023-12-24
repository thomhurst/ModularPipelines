using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medical-imaging", "get-datastore")]
public record AwsMedicalImagingGetDatastoreOptions(
[property: CommandSwitch("--datastore-id")] string DatastoreId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}