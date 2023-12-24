using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectcases", "create-template")]
public record AwsConnectcasesCreateTemplateOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--layout-configuration")]
    public string? LayoutConfiguration { get; set; }

    [CommandSwitch("--required-fields")]
    public string[]? RequiredFields { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}