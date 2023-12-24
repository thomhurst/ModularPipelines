using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectcases", "update-template")]
public record AwsConnectcasesUpdateTemplateOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--template-id")] string TemplateId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--layout-configuration")]
    public string? LayoutConfiguration { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--required-fields")]
    public string[]? RequiredFields { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}