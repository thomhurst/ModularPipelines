using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeartifact", "put-repository-permissions-policy")]
public record AwsCodeartifactPutRepositoryPermissionsPolicyOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--repository")] string Repository,
[property: CliOption("--policy-document")] string PolicyDocument
) : AwsOptions
{
    [CliOption("--domain-owner")]
    public string? DomainOwner { get; set; }

    [CliOption("--policy-revision")]
    public string? PolicyRevision { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}