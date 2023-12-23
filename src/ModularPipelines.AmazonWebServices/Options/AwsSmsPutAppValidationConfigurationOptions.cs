using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sms", "put-app-validation-configuration")]
public record AwsSmsPutAppValidationConfigurationOptions(
[property: CommandSwitch("--app-id")] string AppId
) : AwsOptions
{
    [CommandSwitch("--app-validation-configurations")]
    public string[]? AppValidationConfigurations { get; set; }

    [CommandSwitch("--server-group-validation-configurations")]
    public string[]? ServerGroupValidationConfigurations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}