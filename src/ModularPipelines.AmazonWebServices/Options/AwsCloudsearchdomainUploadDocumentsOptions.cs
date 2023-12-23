using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudsearchdomain", "upload-documents")]
public record AwsCloudsearchdomainUploadDocumentsOptions(
[property: CommandSwitch("--documents")] string Documents,
[property: CommandSwitch("--content-type")] string ContentType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}