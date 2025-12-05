using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "get-mail-domain")]
public record AwsWorkmailGetMailDomainOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--domain-name")] string DomainName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}