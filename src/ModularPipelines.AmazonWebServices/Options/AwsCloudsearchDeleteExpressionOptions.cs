using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudsearch", "delete-expression")]
public record AwsCloudsearchDeleteExpressionOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--expression-name")] string ExpressionName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}