using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeartifact", "associate-external-connection")]
public record AwsCodeartifactAssociateExternalConnectionOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--repository")] string Repository,
[property: CommandSwitch("--external-connection")] string ExternalConnection
) : AwsOptions
{
    [CommandSwitch("--domain-owner")]
    public string? DomainOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}