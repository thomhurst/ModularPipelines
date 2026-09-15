using System.Collections;
using System.ComponentModel.DataAnnotations;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    [MatrixDataSource]
    public async Task Alternative_Joined_Options_Select_The_Joined_Renderer_View(
        [Matrix("ordinary", "key-values", "characters")] string view,
        [Matrix(false, true)] bool hasValues)
    {
        var options = Compile(await GenerateAlternativeCollection(false, "IEnumerable<string>?", collectionSeparator: ","))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var input = view switch
        {
            "key-values" => new JoinedKeyValues(hasValues),
            "characters" => new JoinedCharacterValues(hasValues),
            _ => new JoinedOrdinaryValues(hasValues),
        };
        options.GetProperty("Values")!.SetValue(instance, input);
        string[] expected = (view, hasValues) switch
        {
            ("characters", _) => ["--requirement", hasValues ? "scalar" : ""],
            ("key-values", true) => ["--requirement", "first=second,third=fourth"],
            ("ordinary", true) => ["--requirement", "first,second"],
            _ => [],
        };
        for (var pass = 0; pass < 2; pass++)
        {
            var valid = !((IValidatableObject) instance).Validate(new(instance)).Any();
            await Assert.That(valid).IsEqualTo(expected.Length > 0);
            await Assert.That(RenderAlternativeCollection(instance, false, collectionSeparator: ",")).IsEquivalentTo(expected);
        }

        await Assert.That(input.PairEnumerationCount).IsEqualTo(0);
        await Assert.That(input.EnumerationCount).IsEqualTo(view == "ordinary" ? 1 : 0);
        if (input is JoinedKeyValues keys)
        {
            await Assert.That(keys.KeyEnumerationCount).IsEqualTo(view == "key-values" ? 1 : 0);
        }
    }

    [Test]
    [Arguments("=", OptionFormat.EqualsSeparated)]
    [Arguments(":", OptionFormat.ColonSeparated)]
    [Arguments("", OptionFormat.NoSeparator)]
    public async Task Alternative_Joined_Options_Preserve_Invalid_Pair_Format_Errors(string valueSeparator, OptionFormat format)
    {
        var options = Compile(await GenerateAlternativeCollection(false, "IEnumerable<string>?", collectionSeparator: ",", valueSeparator: valueSeparator))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var input = new JoinedOrdinaryValues(true);
        options.GetProperty("Values")!.SetValue(instance, input);
        await Assert.That(() => RenderAlternativeCollection(instance, false, collectionSeparator: ",", format: format))
            .Throws<InvalidOperationException>();
    }

    [Test]
    [Arguments("=", OptionFormat.EqualsSeparated)]
    [Arguments(":", OptionFormat.ColonSeparated)]
    [Arguments("", OptionFormat.NoSeparator)]
    public async Task Alternative_Joined_Options_Snapshot_NonPair_Inputs_With_Other_Formats(string valueSeparator, OptionFormat format)
    {
        var options = Compile(await GenerateAlternativeCollection(false, "IEnumerable<string>?", collectionSeparator: ",", valueSeparator: valueSeparator))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var input = new SingleUseAlternativeValues();
        options.GetProperty("Values")!.SetValue(instance, input);
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
            await Assert.That(RenderAlternativeCollection(instance, false, collectionSeparator: ",", format: format))
                .IsEquivalentTo([$"--requirement{valueSeparator}first,second"]);
        }

        await Assert.That(input.EnumerationCount).IsEqualTo(1);
    }

    private class JoinedOrdinaryValues(bool hasValues) : IEnumerable<string>, IEnumerable<CliValuePair>
    {
        public int EnumerationCount { get; private set; }

        public int PairEnumerationCount { get; private set; }

        public IEnumerator<string> GetEnumerator()
        {
            if (++EnumerationCount > 1)
            {
                throw new InvalidOperationException("Ordinary input can only be enumerated once.");
            }

            if (hasValues)
            {
                yield return "first";
                yield return "second";
            }
        }

        IEnumerator<CliValuePair> IEnumerable<CliValuePair>.GetEnumerator()
        {
            PairEnumerationCount++;
            yield return new("unrelated", "pair");
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private class JoinedKeyValues(bool hasValues) : JoinedOrdinaryValues(true), IEnumerable<KeyValue>
    {
        public int KeyEnumerationCount { get; private set; }

        IEnumerator<KeyValue> IEnumerable<KeyValue>.GetEnumerator()
        {
            if (++KeyEnumerationCount > 1)
            {
                throw new InvalidOperationException("KeyValue input can only be enumerated once.");
            }

            if (hasValues)
            {
                yield return new("first", "second");
                yield return new("third", "fourth");
            }
        }
    }

    private sealed class JoinedCharacterValues(bool hasValues) : JoinedKeyValues(true), IEnumerable<char>
    {
        public override string ToString() => hasValues ? "scalar" : "";

        IEnumerator<char> IEnumerable<char>.GetEnumerator() => throw new InvalidOperationException("Character view must not be enumerated.");
    }
}
