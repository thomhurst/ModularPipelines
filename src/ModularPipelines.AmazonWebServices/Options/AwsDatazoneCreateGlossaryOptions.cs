using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "create-glossary")]
public record AwsDatazoneCreateGlossaryOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--name")] string Name,
[property: CliOption("--owning-project-identifier")] string OwningProjectIdentifier
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}