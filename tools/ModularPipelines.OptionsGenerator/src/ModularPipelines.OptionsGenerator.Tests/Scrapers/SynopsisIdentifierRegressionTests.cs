using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class SynopsisIdentifierRegressionTests
{
    [Test]
    [Arguments("404-document", "Number404Document")]
    [Arguments("9p", "Number9p")]
    [Arguments("123", "Number123")]
    public async Task Leading_Digits_Have_A_Readable_Prefix(string input, string expected)
    {
        await Assert.That(GeneratorUtils.ToPascalCase(input)).IsEqualTo(expected);
    }

    [Test]
    [Arguments("X_1 ... X_N", "X")]
    [Arguments("KEY_1=VAL_1 ... KEY_N=VAL_N", "KeyVal")]
    [Arguments("KEY_1=VAL_1:TAINT_EFFECT_1 ... KEY_N=VAL_N:TAINT_EFFECT_N", "KeyValTaintEffect")]
    [Arguments("[X_1 ... X_N]", "X")]
    public async Task Numbered_Ranges_Form_One_List(string operands, string name)
    {
        var usage = UsageSynopsisParser.Parse($"Usage: tool run {operands} TAIL", ["tool", "run"]);
        await Assert.That(usage.PositionalArguments.Count).IsEqualTo(2);
        var repeated = usage.PositionalArguments[0];
        await Assert.That(repeated.PropertyName).IsEqualTo(name);
        await Assert.That(repeated.IsVariadic).IsTrue();
        await Assert.That(repeated.IsRequired).IsEqualTo(!operands.StartsWith('['));
        await Assert.That(usage.PositionalArguments[1].PropertyName).IsEqualTo("Tail");
        await Assert.That(usage.PositionalArguments[1].PositionIndex).IsEqualTo(1);
    }

    [Test]
    [Arguments("X_1 ... Y_N")]
    [Arguments("X_1 Y_N")]
    [Arguments("X_1 ... X_NEXT")]
    public async Task Unrelated_Endpoints_Remain_Distinct(string operands)
    {
        var usage = UsageSynopsisParser.Parse($"Usage: tool run {operands}", ["tool", "run"]);
        await Assert.That(usage.PositionalArguments.Count).IsEqualTo(2);
    }

    [Test]
    public async Task Numbered_Switches_Are_Not_Operand_Ranges()
    {
        var usage = UsageSynopsisParser.Parse("Usage: tool run --x_1 ... --x_N", ["tool", "run"]);
        await Assert.That(usage.RequiredOptionSwitches.ToArray()).IsEquivalentTo(["--x_1", "--x_N"]);
    }

    [Test]
    public async Task File_Option_Alternative_Preserves_Resource_Operands()
    {
        var usage = UsageSynopsisParser.Parse("Usage: tool run (-f FILENAME | TYPE NAME) VALUE", ["tool", "run"]);
        await Assert.That(usage.PositionalArguments.Select(argument => argument.PropertyName).ToArray())
            .IsEquivalentTo(["Type", "Name", "Value"]);
        await Assert.That(usage.PositionalArguments[0].IsRequired).IsFalse();
        await Assert.That(usage.PositionalArguments[1].IsRequired).IsFalse();
    }
}
