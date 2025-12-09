using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sns", "get-sms-attributes")]
public record AwsSnsGetSmsAttributesOptions : AwsOptions
{
    [CliOption("--attributes")]
    public string[]? Attributes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}