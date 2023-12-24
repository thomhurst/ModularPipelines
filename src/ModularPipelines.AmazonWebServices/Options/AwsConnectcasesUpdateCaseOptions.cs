using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectcases", "update-case")]
public record AwsConnectcasesUpdateCaseOptions(
[property: CommandSwitch("--case-id")] string CaseId,
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--fields")] string[] Fields
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}