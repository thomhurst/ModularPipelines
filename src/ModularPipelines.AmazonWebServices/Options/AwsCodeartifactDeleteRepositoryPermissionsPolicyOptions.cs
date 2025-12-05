using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeartifact", "delete-repository-permissions-policy")]
public record AwsCodeartifactDeleteRepositoryPermissionsPolicyOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--repository")] string Repository
) : AwsOptions
{
    [CliOption("--domain-owner")]
    public string? DomainOwner { get; set; }

    [CliOption("--policy-revision")]
    public string? PolicyRevision { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}