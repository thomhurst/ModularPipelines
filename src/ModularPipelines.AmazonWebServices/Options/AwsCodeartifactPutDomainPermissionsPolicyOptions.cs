using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeartifact", "put-domain-permissions-policy")]
public record AwsCodeartifactPutDomainPermissionsPolicyOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--policy-document")] string PolicyDocument
) : AwsOptions
{
    [CommandSwitch("--domain-owner")]
    public string? DomainOwner { get; set; }

    [CommandSwitch("--policy-revision")]
    public string? PolicyRevision { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}