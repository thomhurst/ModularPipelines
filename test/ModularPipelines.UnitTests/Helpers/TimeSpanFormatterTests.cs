using ModularPipelines.Helpers;

namespace ModularPipelines.UnitTests.Helpers;

public class TimeSpanFormatterTests
{
    [Test]
    [Arguments(0, 0, 0, 849, "849ms")]
    [Arguments(0, 0, 23, 737, "23s & 737ms")]
    [Arguments(0, 9, 42, 0, "9m & 42s")]
    [Arguments(2, 3, 0, 0, "2h & 3m")]
    public async Task ToDisplayString_UsesOneReadableFormat(
        int hours,
        int minutes,
        int seconds,
        int milliseconds,
        string expected)
    {
        var duration = new TimeSpan(0, hours, minutes, seconds, milliseconds);

        await Assert.That(duration.ToDisplayString()).IsEqualTo(expected);
    }
}
