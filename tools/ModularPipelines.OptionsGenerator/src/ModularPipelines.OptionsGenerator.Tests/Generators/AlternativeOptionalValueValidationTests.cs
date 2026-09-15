using System.Collections;
using System.ComponentModel.DataAnnotations;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    public async Task Alternative_Optional_Values_Normalize_Default_Immutable_Array()
    {
        var options = Compile(await GenerateAlternativeCollection(false, "IEnumerable<string>?", valueArity: CliOptionValueArity.Optional))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        options.GetProperty("Values")!.SetValue(instance, default(System.Collections.Immutable.ImmutableArray<CliOptionValue>));
        await Assert.That(((IValidatableObject) instance).Validate(new(instance)).Any()).IsTrue();
        await Assert.That(RenderAlternativeCollection(instance, false, CliOptionValueArity.Optional)).IsEmpty();
    }

    [Test]
    [MatrixDataSource]
    public async Task Alternative_Optional_Values_Use_Only_Their_Own_Renderer_View(
        [Matrix("pairs", "key-values", "characters")] string view,
        [Matrix("empty", "null-only", "bare", "value", "empty-value")] string payload)
    {
        var options = Compile(await GenerateAlternativeCollection(false, "IEnumerable<string>?", valueArity: CliOptionValueArity.Optional))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        SingleUseOptionalValues input = view switch
        {
            "pairs" => new OptionalValuesWithPairs(payload),
            "key-values" => new OptionalValuesWithKeyValues(payload),
            _ => new OptionalValuesWithCharacters(payload),
        };
        options.GetProperty("Values")!.SetValue(instance, input);
        string[] expected = payload switch
        {
            "bare" => ["--requirement"],
            "value" => ["--requirement", "value"],
            "empty-value" => ["--requirement", ""],
            _ => [],
        };
        for (var pass = 0; pass < 2; pass++)
        {
            var valid = !((IValidatableObject) instance).Validate(new(instance)).Any();
            await Assert.That(valid).IsEqualTo(expected.Length > 0);
            await Assert.That(RenderAlternativeCollection(instance, false, CliOptionValueArity.Optional)).IsEquivalentTo(expected);
        }

        await Assert.That(input.EnumerationCount).IsEqualTo(1);
        await Assert.That(input.OtherEnumerationCount).IsEqualTo(0);
    }

    private class SingleUseOptionalValues(string payload) : IEnumerable<CliOptionValue>
    {
        public int EnumerationCount { get; private set; }

        public int OtherEnumerationCount { get; protected set; }

        public IEnumerator<CliOptionValue> GetEnumerator()
        {
            if (++EnumerationCount > 1)
            {
                throw new InvalidOperationException("Optional-value input can only be enumerated once.");
            }

            if (payload != "empty")
            {
                yield return payload switch
                {
                    "null-only" => null!,
                    "bare" => CliOptionValue.Bare,
                    "empty-value" => (CliOptionValue) "",
                    _ => (CliOptionValue) "value",
                };
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private sealed class OptionalValuesWithPairs(string payload) : SingleUseOptionalValues(payload), IEnumerable<CliValuePair>
    {
        IEnumerator<CliValuePair> IEnumerable<CliValuePair>.GetEnumerator()
        {
            OtherEnumerationCount++;
            yield return new("unrelated", "pair");
        }
    }

    private sealed class OptionalValuesWithKeyValues(string payload) : SingleUseOptionalValues(payload), IEnumerable<KeyValue>
    {
        IEnumerator<KeyValue> IEnumerable<KeyValue>.GetEnumerator()
        {
            OtherEnumerationCount++;
            yield return new("unrelated", "value");
        }
    }

    private sealed class OptionalValuesWithCharacters(string payload) : SingleUseOptionalValues(payload), IEnumerable<char>
    {
        public override string ToString() => "unrelated scalar";

        IEnumerator<char> IEnumerable<char>.GetEnumerator()
        {
            OtherEnumerationCount++;
            yield return 'x';
        }
    }
}
