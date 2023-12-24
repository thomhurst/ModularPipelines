using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sdb", "get-attributes")]
public record AwsSdbGetAttributesOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--item-name")] string ItemName
) : AwsOptions
{
    [CommandSwitch("--attribute-names")]
    public string[]? AttributeNames { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}