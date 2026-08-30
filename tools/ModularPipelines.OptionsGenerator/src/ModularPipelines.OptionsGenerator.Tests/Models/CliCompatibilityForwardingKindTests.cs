using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Models;

public class CliCompatibilityForwardingKindTests
{
    [Test]
    public async Task Established_Member_Ordinals_Remain_Stable()
    {
        var ordinals = Enum.GetValues<CliCompatibilityForwardingKind>()
            .Select(value => $"{value}:{(int) value}");

        await Assert.That(ordinals).IsEquivalentTo([
            "Direct:0",
            "ScalarToCollection:1",
            "NullableInt32ToString:2",
            "NullableBooleanToString:3",
            "NullableStringToRequiredString:4",
            "NullableInt32ToRequiredString:5",
            "NullableInt32ToStringCollection:6",
            "NullableStringToCliOptionValue:7",
            "NullableInt32ToCliOptionValue:8",
            "NullableBooleanToStringCollection:9",
            "NullableBooleanToLocalBackendString:10",
        ]);
    }
}
