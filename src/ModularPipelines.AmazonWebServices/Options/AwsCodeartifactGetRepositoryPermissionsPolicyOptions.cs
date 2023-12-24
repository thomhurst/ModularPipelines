using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeartifact", "get-repository-permissions-policy")]
public record AwsCodeartifactGetRepositoryPermissionsPolicyOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--repository")] string Repository
) : AwsOptions
{
    [CommandSwitch("--domain-owner")]
    public string? DomainOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}