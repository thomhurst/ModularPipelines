using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeartifact", "get-repository-endpoint")]
public record AwsCodeartifactGetRepositoryEndpointOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--repository")] string Repository,
[property: CommandSwitch("--format")] string Format
) : AwsOptions
{
    [CommandSwitch("--domain-owner")]
    public string? DomainOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}