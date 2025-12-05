using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "remove-attributes")]
public record AwsPinpointRemoveAttributesOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--attribute-type")] string AttributeType,
[property: CliOption("--update-attributes-request")] string UpdateAttributesRequest
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}