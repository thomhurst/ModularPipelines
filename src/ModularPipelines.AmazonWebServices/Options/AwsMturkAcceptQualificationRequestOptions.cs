using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "accept-qualification-request")]
public record AwsMturkAcceptQualificationRequestOptions(
[property: CommandSwitch("--qualification-request-id")] string QualificationRequestId
) : AwsOptions
{
    [CommandSwitch("--integer-value")]
    public int? IntegerValue { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}