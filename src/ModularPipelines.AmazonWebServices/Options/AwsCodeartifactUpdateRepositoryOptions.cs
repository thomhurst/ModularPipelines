using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeartifact", "update-repository")]
public record AwsCodeartifactUpdateRepositoryOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--repository")] string Repository
) : AwsOptions
{
    [CliOption("--domain-owner")]
    public string? DomainOwner { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--upstreams")]
    public string[]? Upstreams { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}