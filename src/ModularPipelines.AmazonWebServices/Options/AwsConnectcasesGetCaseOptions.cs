using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectcases", "get-case")]
public record AwsConnectcasesGetCaseOptions(
[property: CommandSwitch("--case-id")] string CaseId,
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--fields")] string[] Fields
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}