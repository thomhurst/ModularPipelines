using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pca-connector-ad", "update-template")]
public record AwsPcaConnectorAdUpdateTemplateOptions(
[property: CommandSwitch("--template-arn")] string TemplateArn
) : AwsOptions
{
    [CommandSwitch("--definition")]
    public string? Definition { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}