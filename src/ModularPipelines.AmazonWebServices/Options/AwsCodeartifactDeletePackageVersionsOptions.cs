using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeartifact", "delete-package-versions")]
public record AwsCodeartifactDeletePackageVersionsOptions(
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

    [CommandSwitch("--expected-status")]
    public string? ExpectedStatus { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}