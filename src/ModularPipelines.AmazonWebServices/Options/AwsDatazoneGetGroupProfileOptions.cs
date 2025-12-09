using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "get-group-profile")]
public record AwsDatazoneGetGroupProfileOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--group-identifier")] string GroupIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}