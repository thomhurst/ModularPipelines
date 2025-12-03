using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firebase", "test", "android", "models", "describe")]
public record GcloudFirebaseTestAndroidModelsDescribeOptions(
[property: CliArgument] string ModelId
) : GcloudOptions;