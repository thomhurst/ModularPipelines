using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sdb", "delete-attributes")]
public record AwsSdbDeleteAttributesOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--item-name")] string ItemName
) : AwsOptions
{
    [CommandSwitch("--attributes")]
    public string[]? Attributes { get; set; }

    [CommandSwitch("--expected")]
    public string? Expected { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}