using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectcases", "batch-get-field")]
public record AwsConnectcasesBatchGetFieldOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--fields")] string[] Fields
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}