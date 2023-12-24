using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("textract", "get-adapter-version")]
public record AwsTextractGetAdapterVersionOptions(
[property: CommandSwitch("--adapter-id")] string AdapterId,
[property: CommandSwitch("--adapter-version")] string AdapterVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}