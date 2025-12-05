using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("accessanalyzer", "check-access-not-granted")]
public record AwsAccessanalyzerCheckAccessNotGrantedOptions(
[property: CliOption("--policy-document")] string PolicyDocument,
[property: CliOption("--access")] string[] Access,
[property: CliOption("--policy-type")] string PolicyType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}