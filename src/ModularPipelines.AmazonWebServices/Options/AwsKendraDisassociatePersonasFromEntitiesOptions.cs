using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "disassociate-personas-from-entities")]
public record AwsKendraDisassociatePersonasFromEntitiesOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--entity-ids")] string[] EntityIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}