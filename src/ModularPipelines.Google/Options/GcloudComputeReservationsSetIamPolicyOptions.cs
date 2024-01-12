using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "reservations", "set-iam-policy")]
public record GcloudComputeReservationsSetIamPolicyOptions(
[property: PositionalArgument] string Reservation,
[property: PositionalArgument] string Zone,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;