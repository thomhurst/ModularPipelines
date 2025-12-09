using ModularPipelines.Attributes;
using ModularPipelines.Helpers.Internal;

namespace ModularPipelines.UnitTests.Attributes;

public class EnumValueAttributeTests
{
    private readonly CommandModelProvider _modelProvider = new();
    private readonly CommandArgumentBuilder _argumentBuilder = new();

    private List<string> BuildArguments(object optionsObject)
    {
        var model = _modelProvider.GetCommandModel(optionsObject.GetType());
        return _argumentBuilder.BuildArguments(model, optionsObject);
    }

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

        var list = BuildArguments(options);
        await Assert.That(list).Contains("--number");
        await Assert.That(list).Contains(expected);
        await Assert.That(list).IsEquivalentTo(["--number", expected]);
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
        [CliOption("--number")]
        public Number Number { get; set; }
    }
}
