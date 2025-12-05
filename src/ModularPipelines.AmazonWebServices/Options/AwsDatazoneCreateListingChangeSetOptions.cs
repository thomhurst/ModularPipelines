using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "create-listing-change-set")]
public record AwsDatazoneCreateListingChangeSetOptions(
[property: CliOption("--action")] string Action,
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--entity-identifier")] string EntityIdentifier,
[property: CliOption("--entity-type")] string EntityType
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--entity-revision")]
    public string? EntityRevision { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}