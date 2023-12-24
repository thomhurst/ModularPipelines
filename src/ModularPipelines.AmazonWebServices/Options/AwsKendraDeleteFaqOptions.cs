using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "delete-faq")]
public record AwsKendraDeleteFaqOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--index-id")] string IndexId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}