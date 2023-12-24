using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53domains", "update-tags-for-domain")]
public record AwsRoute53domainsUpdateTagsForDomainOptions(
[property: CommandSwitch("--domain-name")] string DomainName
) : AwsOptions
{
    [CommandSwitch("--tags-to-update")]
    public string[]? TagsToUpdate { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}