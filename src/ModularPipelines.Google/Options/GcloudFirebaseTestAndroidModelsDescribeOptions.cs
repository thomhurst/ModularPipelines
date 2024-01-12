using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firebase", "test", "android", "models", "describe")]
public record GcloudFirebaseTestAndroidModelsDescribeOptions(
[property: PositionalArgument] string ModelId
) : GcloudOptions;