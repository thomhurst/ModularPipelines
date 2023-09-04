using ModularPipelines.Attributes;
using ModularPipelines.Helpers;

namespace ModularPipelines.UnitTests.Attributes;

public class EnumValueAttributeTests
{
    [TestCase(Number.One, "1")]
    [TestCase(Number.Two, "2")]
    [TestCase(Number.Three, "3")]
    public void Can_Parse_EnumValueAttribute(Number number, string expected)
    {
        var options = new NumberWrapper
        {
            Number = number
        };
        
        var list = new List<string>();
        
        CommandOptionsObjectArgumentParser.AddArgumentsFromOptionsObject(list, options);
        
        Assert.That(list, Does.Contain("--number"));
        Assert.That(list, Does.Contain(expected));
        
        Assert.That(list, Is.EquivalentTo(new List<string> { "--number", expected }));
    }
    
    public enum Number
    {
        [EnumValue("1")]
        One,
        [EnumValue("2")]
        Two,
        [EnumValue("3")]
        Three
    }

    private record NumberWrapper
    {
        [CommandSwitch("--number")]
        public Number Number { get; set; }
    }
}