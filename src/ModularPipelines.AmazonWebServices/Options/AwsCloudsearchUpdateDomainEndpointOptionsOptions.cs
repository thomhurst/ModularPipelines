using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudsearch", "update-domain-endpoint-options")]
public record AwsCloudsearchUpdateDomainEndpointOptionsOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--domain-endpoint-options")] string DomainEndpointOptions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}