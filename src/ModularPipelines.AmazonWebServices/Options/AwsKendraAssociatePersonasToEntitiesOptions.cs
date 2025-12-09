using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "associate-personas-to-entities")]
public record AwsKendraAssociatePersonasToEntitiesOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--personas")] string[] Personas
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}