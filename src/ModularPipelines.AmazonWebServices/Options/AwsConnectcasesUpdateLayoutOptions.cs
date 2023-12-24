using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectcases", "update-layout")]
public record AwsConnectcasesUpdateLayoutOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--layout-id")] string LayoutId
) : AwsOptions
{
    [CommandSwitch("--content")]
    public string? Content { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}