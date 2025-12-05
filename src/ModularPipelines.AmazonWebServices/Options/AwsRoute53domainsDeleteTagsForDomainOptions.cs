using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53domains", "delete-tags-for-domain")]
public record AwsRoute53domainsDeleteTagsForDomainOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--tags-to-delete")] string[] TagsToDelete
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}