using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sdb", "batch-put-attributes")]
public record AwsSdbBatchPutAttributesOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--items")] string[] Items
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}