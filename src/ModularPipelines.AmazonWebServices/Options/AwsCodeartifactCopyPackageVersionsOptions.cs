using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeartifact", "copy-package-versions")]
public record AwsCodeartifactCopyPackageVersionsOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--source-repository")] string SourceRepository,
[property: CommandSwitch("--destination-repository")] string DestinationRepository,
[property: CommandSwitch("--format")] string Format,
[property: CommandSwitch("--package")] string Package
) : AwsOptions
{
    [CommandSwitch("--domain-owner")]
    public string? DomainOwner { get; set; }

    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [CommandSwitch("--versions")]
    public string[]? Versions { get; set; }

    [CommandSwitch("--version-revisions")]
    public IEnumerable<KeyValue>? VersionRevisions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}