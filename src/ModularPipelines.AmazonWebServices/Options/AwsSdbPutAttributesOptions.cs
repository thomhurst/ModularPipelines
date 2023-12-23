using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sdb", "put-attributes")]
public record AwsSdbPutAttributesOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--item-name")] string ItemName,
[property: CommandSwitch("--attributes")] string[] Attributes
) : AwsOptions
{
    [CommandSwitch("--expected")]
    public string? Expected { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}