using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfigdata", "get-latest-configuration")]
public record AwsAppconfigdataGetLatestConfigurationOptions(
[property: CommandSwitch("--configuration-token")] string ConfigurationToken
) : AwsOptions;