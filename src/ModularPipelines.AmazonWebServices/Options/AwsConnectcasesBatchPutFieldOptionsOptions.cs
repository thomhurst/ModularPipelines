using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectcases", "batch-put-field-options")]
public record AwsConnectcasesBatchPutFieldOptionsOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--field-id")] string FieldId,
[property: CommandSwitch("--options")] string[] Options
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}