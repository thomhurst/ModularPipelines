using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectcases", "get-layout")]
public record AwsConnectcasesGetLayoutOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--layout-id")] string LayoutId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}