using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sms", "put-app-validation-configuration")]
public record AwsSmsPutAppValidationConfigurationOptions(
[property: CliOption("--app-id")] string AppId
) : AwsOptions
{
    [CliOption("--app-validation-configurations")]
    public string[]? AppValidationConfigurations { get; set; }

    [CliOption("--server-group-validation-configurations")]
    public string[]? ServerGroupValidationConfigurations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}