using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("accessanalyzer", "check-access-not-granted")]
public record AwsAccessanalyzerCheckAccessNotGrantedOptions(
[property: CommandSwitch("--policy-document")] string PolicyDocument,
[property: CommandSwitch("--access")] string[] Access,
[property: CommandSwitch("--policy-type")] string PolicyType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}