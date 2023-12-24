using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "get-access-key-last-used")]
public record AwsIamGetAccessKeyLastUsedOptions(
[property: CommandSwitch("--access-key-id")] string AccessKeyId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}