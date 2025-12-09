using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeartifact", "copy-package-versions")]
public record AwsCodeartifactCopyPackageVersionsOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--source-repository")] string SourceRepository,
[property: CliOption("--destination-repository")] string DestinationRepository,
[property: CliOption("--format")] string Format,
[property: CliOption("--package")] string Package
) : AwsOptions
{
    [CliOption("--domain-owner")]
    public string? DomainOwner { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--versions")]
    public string[]? Versions { get; set; }

    [CliOption("--version-revisions")]
    public IEnumerable<KeyValue>? VersionRevisions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}