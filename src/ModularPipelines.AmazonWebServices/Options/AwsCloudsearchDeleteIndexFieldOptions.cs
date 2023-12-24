using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudsearch", "delete-index-field")]
public record AwsCloudsearchDeleteIndexFieldOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--index-field-name")] string IndexFieldName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}