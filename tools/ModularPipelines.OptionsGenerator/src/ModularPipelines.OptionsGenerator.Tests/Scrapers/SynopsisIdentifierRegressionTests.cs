using ModularPipelines.OptionsGenerator.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class SynopsisIdentifierRegressionTests
{
    [Test]
    [Arguments("X_1 ... X_N", "X")]
    [Arguments("KEY_1=VAL_1 ... KEY_N=VAL_N", "KeyVal")]
    [Arguments("KEY_1=VAL_1:TAINT_EFFECT_1 ... KEY_N=VAL_N:TAINT_EFFECT_N", "KeyValTaintEffect")]
    [Arguments("[X_1 ... X_N]", "X")]
    [Arguments("(X_1 ... X_N)", "X")]
    [Arguments("[(KEY_1=VAL_1 ... KEY_N=VAL_N)]", "KeyVal")]
    [Arguments("X_1 … X_N", "X")]
    public async Task Numbered_Ranges_Form_One_List(string operands, string name)
    {
        var usage = UsageSynopsisParser.Parse($"Usage: tool run {operands} TAIL", ["tool", "run"]);
        await Assert.That(usage.PositionalArguments.Count).IsEqualTo(2);
        var repeated = usage.PositionalArguments[0];
        await Assert.That(repeated.PropertyName).IsEqualTo(name);
        await Assert.That(repeated.IsVariadic).IsTrue();
        await Assert.That(repeated.CSharpType).IsEqualTo(operands.StartsWith('[') ? "IEnumerable<string>?" : "IEnumerable<string>");
        await Assert.That(repeated.PositionIndex).IsEqualTo(0);
        await Assert.That(repeated.IsRequired).IsEqualTo(!operands.StartsWith('['));
        await Assert.That(usage.PositionalArguments[1].PropertyName).IsEqualTo("Tail");
        await Assert.That(usage.PositionalArguments[1].PositionIndex).IsEqualTo(1);
    }

    [Test]
    [Arguments("X_1 ... Y_N")]
    [Arguments("X_1 Y_N")]
    [Arguments("X_1 ... X_NEXT")]
    [Arguments("X_1 ... X_N_EXTRA")]
    [Arguments("KEY_1=VAL_1 ... KEY_N=OTHER_N")]
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
}
