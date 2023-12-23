using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectcases", "create-related-item")]
public record AwsConnectcasesCreateRelatedItemOptions(
[property: CommandSwitch("--case-id")] string CaseId,
[property: CommandSwitch("--content")] string Content,
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--performed-by")]
    public string? PerformedBy { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}