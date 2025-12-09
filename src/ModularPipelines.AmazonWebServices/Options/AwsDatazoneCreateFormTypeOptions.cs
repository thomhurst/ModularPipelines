using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "create-form-type")]
public record AwsDatazoneCreateFormTypeOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--model")] string Model,
[property: CliOption("--name")] string Name,
[property: CliOption("--owning-project-identifier")] string OwningProjectIdentifier
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}