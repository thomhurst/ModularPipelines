using ModularPipelines.Attributes;
using ModularPipelines.Helpers;
using TUnit.Assertions;
using TUnit.Core;

namespace ModularPipelines.UnitTests.Attributes;

public class EnumValueAttributeTests
{
    [TestWithData(Number.One, "1")]
    [TestWithData(Number.Two, "2")]
    [TestWithData(Number.Three, "3")]
    public void Can_Parse_EnumValueAttribute(Number number, string expected)
    {
        var options = new NumberWrapper
        {
            Number = number,
        };

        var list = new List<string>();

        CommandOptionsObjectArgumentParser.AddArgumentsFromOptionsObject(list, options);
        await Assert.That(list).Does.Contain("--number");
        await Assert.That(list).Does.Contain(expected);
        await Assert.That(list).Is.EquivalentTo(new List<string> { "--number" });
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