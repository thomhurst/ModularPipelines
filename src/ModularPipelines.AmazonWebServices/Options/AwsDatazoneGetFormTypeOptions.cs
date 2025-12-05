using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "get-form-type")]
public record AwsDatazoneGetFormTypeOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--form-type-identifier")] string FormTypeIdentifier
) : AwsOptions
{
    [CliOption("--revision")]
    public string? Revision { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}