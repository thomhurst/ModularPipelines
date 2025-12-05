using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "deregister-from-work-mail")]
public record AwsWorkmailDeregisterFromWorkMailOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--entity-id")] string EntityId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}