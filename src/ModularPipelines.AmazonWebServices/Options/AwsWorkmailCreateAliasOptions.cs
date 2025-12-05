using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "create-alias")]
public record AwsWorkmailCreateAliasOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--entity-id")] string EntityId,
[property: CliOption("--alias")] string Alias
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}