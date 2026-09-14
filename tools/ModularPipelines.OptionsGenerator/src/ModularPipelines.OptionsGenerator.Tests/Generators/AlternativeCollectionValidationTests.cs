using System.Collections;
using System.ComponentModel.DataAnnotations;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Alternative_Collections_Retain_SingleUse_Inputs_After_Validation(bool positional)
    {
        var options = Compile(await GenerateAlternativeCollection(positional, "IEnumerable<string>?"))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var property = options.GetProperty("Values")!;
        var source = new SingleUseAlternativeValues();
        property.SetValue(instance, source);
        var validation = (IValidatableObject) instance;

        for (var pass = 0; pass < 2; pass++)
        {
            await Assert.That(validation.Validate(new(instance))).IsEmpty();
            var retained = ((IEnumerable) property.GetValue(instance)!).Cast<string>().ToArray();
            await Assert.That(retained).IsEquivalentTo(["first", "second"]);
        }

        await Assert.That(source.EnumerationCount).IsEqualTo(1);
        property.SetValue(instance, new SingleUseAlternativeValues());
        await Assert.That(validation.Validate(new(instance))).IsEmpty();
        await Assert.That(((IEnumerable) property.GetValue(instance)!).Cast<string>().ToArray())
            .IsEquivalentTo(["first", "second"]);
        property.SetValue(instance, Array.Empty<string>());
        await Assert.That(validation.Validate(new(instance))).Count().IsEqualTo(1);
        property.SetValue(instance, null);
        await Assert.That(validation.Validate(new(instance))).Count().IsEqualTo(1);
        options.GetProperty("Fallback")!.SetValue(instance, "fallback");
        await Assert.That(validation.Validate(new(instance))).IsEmpty();
    }

    [Test]
    [Arguments("List<string>?")]
    [Arguments("IReadOnlyList<string>?")]
    [Arguments("ISet<string>?")]
    [Arguments("System.Collections.Immutable.ImmutableArray<string>?")]
    [Arguments("System.Collections.IEnumerable?")]
    public async Task Alternative_Collections_Preserve_Collection_Contracts(string collectionType)
    {
        var options = Compile(await GenerateAlternativeCollection(false, collectionType))
            .GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(options)!;
        var property = options.GetProperty("Values")!;
        List<string> supplied = ["first", "second"];
        object input = collectionType switch
        {
            "ISet<string>?" => new HashSet<string>(supplied, StringComparer.OrdinalIgnoreCase),
            "System.Collections.Immutable.ImmutableArray<string>?" => System.Collections.Immutable.ImmutableArray.CreateRange(supplied),
            _ => supplied,
        };
        property.SetValue(instance, input);
        supplied.Clear();
        var validation = (IValidatableObject) instance;
        await Assert.That(validation.Validate(new(instance))).IsEmpty();
        var retained = (IEnumerable) property.GetValue(instance)!;
        await Assert.That(retained.Cast<string>().ToArray()).IsEquivalentTo(["first", "second"]);
        if (retained is HashSet<string> set)
        {
            await Assert.That(set.Comparer).IsSameReferenceAs(StringComparer.OrdinalIgnoreCase);
            set.Clear();
            await Assert.That(validation.Validate(new(instance))).Count().IsEqualTo(1);
        }
        else if (retained is List<string> list)
        {
            list.Clear();
            await Assert.That(validation.Validate(new(instance))).Count().IsEqualTo(1);
        }
    }

    [Test]
    [Arguments("IReadOnlyList<KeyValue>?")]
    [Arguments("List<KeyValue>?")]
    public async Task Alternative_Collections_Retain_Domain_Value_Types(string collectionType)
    {
        var assembly = Compile(await GenerateAlternativeCollection(false, collectionType));
        var options = assembly.GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var elementType = assembly.GetType("ModularPipelines.Models.KeyValue")!;
        var values = Array.CreateInstance(elementType, 2);
        values.SetValue(Activator.CreateInstance(elementType), 0);
        values.SetValue(Activator.CreateInstance(elementType), 1);
        var supplied = Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType), [values]);
        var instance = Activator.CreateInstance(options)!;
        options.GetProperty("Values")!.SetValue(instance, supplied);
        var validation = (IValidatableObject) instance;
        await Assert.That(validation.Validate(new(instance))).IsEmpty();
        var retained = (IEnumerable) options.GetProperty("Values")!.GetValue(instance)!;
        await Assert.That(retained.Cast<object>().ToArray()).IsEquivalentTo(values.Cast<object>());
    }

    private static Task<string> GenerateAlternativeCollection(bool positional, string collectionType)
    {
        List<CliOptionDefinition> options =
            [new() { SwitchName = "--fallback", PropertyName = "Fallback", CSharpType = "string?" }];
        if (!positional)
        {
            options.Add(new() { SwitchName = "--requirement", PropertyName = "Values", CSharpType = collectionType });
        }

        var member = new CliRequiredAlternativeMember
        {
            PropertyName = "Values",
            OptionSwitch = positional ? null : "--requirement",
            PositionalArgumentPhase = positional ? ModularPipelines.Attributes.CommandLinePhase.EarlyOperand : null,
            PositionalArgumentPositionIndex = positional ? 0 : null,
        };
        return Generate(options,
            positional ? [new() { PropertyName = "Values", CSharpType = collectionType, PositionIndex = 0 }] : [],
            [new() { Members = [member, new() { PropertyName = "Fallback", OptionSwitch = "--fallback" }] }]);
    }

    private sealed class SingleUseAlternativeValues : IEnumerable<string>
    {
        public int EnumerationCount { get; private set; }

        public IEnumerator<string> GetEnumerator()
        {
            if (++EnumerationCount != 1)
            {
                throw new InvalidOperationException("Input can only be enumerated once.");
            }

            return ((IEnumerable<string>) new[] { "first", "second" }).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
