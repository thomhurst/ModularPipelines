using System.ComponentModel.DataAnnotations;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    [MatrixDataSource]
    public async Task Validated_String_Collections_Reject_Blank_Entries(
        [Matrix(null, "", " ", "\t", "development")] string? entry,
        [Matrix(false, true)] bool mixed,
        [Matrix(false, true)] bool alternative)
    {
        var options = Compile(await GenerateValidatedStringCollection(alternative))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var enumerationCount = 0;
        IEnumerable<string> Values()
        {
            if (++enumerationCount > 1)
            {
                throw new InvalidOperationException("Input was enumerated twice.");
            }

            if (mixed)
            {
                yield return "testing";
            }

            yield return entry!;
        }

        options.GetProperty("Values")!.SetValue(instance, Values());
        for (var attempt = 0; attempt < 2; attempt++)
        {
            var errors = ((IValidatableObject) instance).Validate(new(instance)).ToArray();
            await Assert.That(errors.Length == 0).IsEqualTo(!string.IsNullOrWhiteSpace(entry));
            if (string.IsNullOrWhiteSpace(entry))
            {
                var blankError = errors.Single(error => error.ErrorMessage == "Values cannot contain null, empty, or whitespace values.");
                await Assert.That(blankError.MemberNames).IsEquivalentTo(["Values"]);
            }
            else
            {
                string[] expected = mixed
                    ? ["--requirement", "testing", "--requirement", entry!]
                    : ["--requirement", entry!];
                await Assert.That(RenderAlternativeCollection(instance, positional: false)).IsEquivalentTo(expected);
            }
        }

        await Assert.That(enumerationCount).IsEqualTo(1);
    }

    [Test]
    public async Task Validated_Optional_String_Collections_Allow_Null_And_Empty_Collections()
    {
        var options = Compile(await GenerateValidatedStringCollection())
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        foreach (var values in new IEnumerable<string>?[] { null, [] })
        {
            options.GetProperty("Values")!.SetValue(instance, values);
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
            await Assert.That(RenderAlternativeCollection(instance, positional: false)).IsEmpty();
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task String_Collection_Blank_Validation_Is_Opt_In(bool alternative)
    {
        var options = Compile(await GenerateValidatedStringCollection(alternative, rejectBlank: false))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        string[] values = [null!, "", " ", "valid"];
        var property = options.GetProperty("Values")!;
        property.SetValue(instance, values);
        if (alternative)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
        }
        else
        {
            await Assert.That(instance is IValidatableObject).IsFalse();
            await Assert.That(property.GetValue(instance)).IsSameReferenceAs(values);
        }
    }

    [Test]
    [MatrixDataSource]
    public async Task Validated_String_Collections_Preserve_Renderer_Views(
        [Matrix("IEnumerable<string>?", "IReadOnlyList<string>?", "List<string>?")] string collectionType,
        [Matrix(false, true)] bool joined)
    {
        var separator = joined ? "," : null;
        var options = Compile(await GenerateValidatedStringCollection(collectionType: collectionType, collectionSeparator: separator))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var input = new StringRendererViews(singleUse: collectionType != "List<string>?");
        var property = options.GetProperty("Values")!;
        property.SetValue(instance, input);
        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(((IValidatableObject) instance).Validate(new(instance))).IsEmpty();
            string[] expected = joined ? ["--requirement", "typed=value"] : ["--requirement", "typed", "value"];
            await Assert.That(RenderAlternativeCollection(instance, positional: false, collectionSeparator: separator))
                .IsEquivalentTo(expected);
        }

        if (collectionType == "List<string>?")
        {
            // Mutable contracts retain incompatible typed views rather than replace the source.
            await Assert.That(property.GetValue(instance)).IsSameReferenceAs(input);
        }
        else
        {
            await Assert.That(input.PairEnumerationCount).IsEqualTo(joined ? 0 : 1);
            await Assert.That(input.KeyValueEnumerationCount).IsEqualTo(joined ? 1 : 0);
        }
    }

    [Test]
    [Arguments("string[]?")]
    [Arguments("List<string>?")]
    [Arguments("IReadOnlyList<string?>?")]
    [Arguments("System.Collections.Immutable.ImmutableArray<string>?")]
    [Arguments("System.Collections.IEnumerable?")]
    public async Task Validated_String_Collections_Support_Declared_Collection_Contracts(string collectionType)
    {
        var options = Compile(await GenerateValidatedStringCollection(collectionType: collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        foreach (var entry in new string?[] { null, "", " ", "valid" })
        {
            string[] values = ["first", entry!];
            object input = collectionType switch
            {
                "List<string>?" => values.ToList(),
                "System.Collections.Immutable.ImmutableArray<string>?" => System.Collections.Immutable.ImmutableArray.CreateRange(values),
                _ => values,
            };
            options.GetProperty("Values")!.SetValue(instance, input);
            await Assert.That(((IValidatableObject) instance).Validate(new(instance)).Any())
                .IsEqualTo(string.IsNullOrWhiteSpace(entry));
        }
    }

    private static Task<string> GenerateValidatedStringCollection(
        bool alternative = false, bool rejectBlank = true, string collectionType = "IEnumerable<string>?", string? collectionSeparator = null) =>
        Generate([new CliOptionDefinition
        {
            SwitchName = "--requirement",
            PropertyName = "Values",
            CSharpType = collectionType,
            AcceptsMultipleValues = true,
            RejectBlankCollectionValues = rejectBlank,
            CollectionSeparator = collectionSeparator,
        }], alternativeGroups: alternative
            ? [new() { Members = [new() { PropertyName = "Values", OptionSwitch = "--requirement" }] }]
            : []);
}
