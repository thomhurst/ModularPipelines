using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "delete-form-type")]
public record AwsDatazoneDeleteFormTypeOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--form-type-identifier")] string FormTypeIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}