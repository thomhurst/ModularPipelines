using ModularPipelines.Attributes;
using ModularPipelines.Helpers;

namespace ModularPipelines.UnitTests.Attributes;

public class EnumValueAttributeTests
{
    [Test]
    [Arguments(Number.One, "1")]
    [Arguments(Number.Two, "2")]
    [Arguments(Number.Three, "3")]
    public async Task Can_Parse_EnumValueAttribute(Number number, string expected)
    {
        var options = new NumberWrapper
        {
            Number = number,
        };

        var list = new List<string>();

        CommandOptionsObjectArgumentParser.AddArgumentsFromOptionsObject(list, options);
        await Assert.That(list).Contains("--number");
        await Assert.That(list).Contains(expected);
        await Assert.That(list).IsEquivalentTo(new List<string> { "--number", expected });
    }

    public enum Number
    {
        [EnumValue("1")]
        One,
        [EnumValue("2")]
        Two,
        [EnumValue("3")]
        Three,
    }

    private record NumberWrapper
    {
        [CommandSwitch("--number")]
        public Number Number { get; set; }
    }
}