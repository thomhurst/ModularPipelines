using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeartifact", "dispose-package-versions")]
public record AwsCodeartifactDisposePackageVersionsOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--repository")] string Repository,
[property: CommandSwitch("--format")] string Format,
[property: CommandSwitch("--package")] string Package,
[property: CommandSwitch("--versions")] string[] Versions
) : AwsOptions
{
    [CommandSwitch("--domain-owner")]
    public string? DomainOwner { get; set; }

    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [CommandSwitch("--version-revisions")]
    public IEnumerable<KeyValue>? VersionRevisions { get; set; }

    [CommandSwitch("--expected-status")]
    public string? ExpectedStatus { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}