using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeartifact", "update-package-versions-status")]
public record AwsCodeartifactUpdatePackageVersionsStatusOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--repository")] string Repository,
[property: CliOption("--format")] string Format,
[property: CliOption("--package")] string Package,
[property: CliOption("--versions")] string[] Versions,
[property: CliOption("--target-status")] string TargetStatus
) : AwsOptions
{
    [CliOption("--domain-owner")]
    public string? DomainOwner { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--version-revisions")]
    public IEnumerable<KeyValue>? VersionRevisions { get; set; }

    [CliOption("--expected-status")]
    public string? ExpectedStatus { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}