using System.Collections;
using Microsoft.Extensions.Logging;
using ModularPipelines.Console;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using ModularPipelines.Secrets;
using Moq;

namespace ModularPipelines.UnitTests.Logging;

public class StructuredLogSecretTests
{
    private const string Secret = "structured-secret";

    [Test]
    public async Task BufferedEvent_ExportsSanitizedArrayAndListElements()
    {
        object?[] args = ["--token", Secret, new List<object?> { Secret, 42, null }];
        var obfuscator = CreateObfuscator();
        var logger = new Mock<ILogger>();
        logger.Object.LogInformation("Args {Args}", (object) args);
        var state = logger.Invocations.Single().Arguments[2]!;
        logger.Invocations.Clear();
        var sanitized = new FormattedLogValuesObfuscator(obfuscator).TryObfuscateValues(state);
        var logEvent = new BufferedLogEvent<object>(
            LogLevel.Information, default, state, sanitized, null,
            (value, _) => value.ToString()!, obfuscator);

        logEvent.WriteTo(logger.Object);

        var exported = (IReadOnlyList<KeyValuePair<string, object?>>) logger.Invocations.Single().Arguments[2]!;
        var elements = ((IEnumerable) exported[0].Value!).Cast<object?>().ToArray();
        await Assert.That(elements[0]).IsEqualTo("--token");
        await Assert.That(elements[1]).IsEqualTo("**********");
        var nested = ((IEnumerable) elements[2]!).Cast<object?>().ToArray();
        await Assert.That(nested[0]).IsEqualTo("**********");
        await Assert.That(nested[1]).IsEqualTo(42);
        await Assert.That(nested[2]).IsNull();
        await Assert.That(args[1]).IsEqualTo(Secret);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task BufferedEvent_MasksCharacterCollectionsAsOneSequence(int collectionKind)
    {
        var characters = Secret.ToCharArray();
        object value = collectionKind switch
        {
            0 => characters,
            1 => characters.ToList(),
            _ => characters.Select(character => character),
        };
        var obfuscator = CreateObfuscator();
        var logger = new Mock<ILogger>();
        logger.Object.LogInformation("Value {Value}", (object) new[] { value });
        var state = logger.Invocations.Single().Arguments[2]!;
        logger.Invocations.Clear();
        var sanitized = new FormattedLogValuesObfuscator(obfuscator).TryObfuscateValues(state);
        var logEvent = new BufferedLogEvent<object>(
            LogLevel.Information, default, state, sanitized, null,
            (item, _) => item.ToString()!, obfuscator);

        logEvent.WriteTo(logger.Object);

        var exported = (IReadOnlyList<KeyValuePair<string, object?>>) logger.Invocations.Single().Arguments[2]!;
        var nested = ((IEnumerable) exported[0].Value!).Cast<object>().Single();
        var maskedCharacters = ((IEnumerable) nested).Cast<char>().ToArray();
        await Assert.That(new string(maskedCharacters)).IsEqualTo("**********");
        await Assert.That(new string(characters)).IsEqualTo(Secret);
    }

    [Test]
    [Arguments("")]
    [Arguments("ordinary")]
    public async Task CharacterCollection_IsAnIndependentSnapshot(string text)
    {
        var characters = text.ToCharArray();
        var sanitized = new FormattedLogValuesObfuscator(CreateObfuscator()).TryObfuscateValues(characters);
        Array.Fill(characters, 'x');
        await Assert.That(new string(((IEnumerable) sanitized).Cast<char>().ToArray())).IsEqualTo(text);
        if (characters.Length > 0)
        {
            await Assert.That(sanitized).IsNotSameReferenceAs(characters);
        }
    }

    [Test]
    public async Task CharacterCollections_RespectTheSharedBudget()
    {
        object[] values = [new string('x', 1000).ToCharArray(), Secret.ToCharArray(), Secret.ToCharArray()];
        var sanitized = (object[]) new FormattedLogValuesObfuscator(CreateObfuscator()).TryObfuscateValues(values);
        await Assert.That(new string((char[]) sanitized[1])).IsEqualTo("**********");
        var truncated = ((IEnumerable) sanitized[2]).Cast<object>().Single();
        await Assert.That(truncated).IsEqualTo("[Truncated log value]");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task OversizedCharacterCollections_DiscardPartialSequences(bool infinite)
    {
        IEnumerable<char> characters = infinite ? InfiniteCharacters() : new string('x', 1024).ToCharArray();
        var sanitized = new FormattedLogValuesObfuscator(CreateObfuscator()).TryObfuscateValues(characters);
        await Assert.That(((IEnumerable) sanitized).Cast<object>().Single()).IsEqualTo("[Truncated log value]");
    }

    [Test]
    public async Task CharacterMaskExpansion_RespectsTheBudget()
    {
        var obfuscator = CreateObfuscator(maskingOptions: new SecretMaskingOptions { MaskValue = new string('*', 1024) });
        var sanitized = new FormattedLogValuesObfuscator(obfuscator).TryObfuscateValues(Secret.ToCharArray());
        await Assert.That(((IEnumerable) sanitized).Cast<object>().Single()).IsEqualTo("[Truncated log value]");
    }

    [Test]
    public async Task ThrowingCharacterCollection_DiscardsPartialSequences()
    {
        var sanitized = new FormattedLogValuesObfuscator(CreateObfuscator()).TryObfuscateValues(ThrowingCharacters());
        await Assert.That(sanitized).IsEqualTo("[Unavailable log value]");
    }

    [Test]
    public async Task BranchingSharedCollections_RespectTheSharedBudget()
    {
        object value = new object[] { Secret };
        for (var depth = 0; depth < 14; depth++)
        {
            value = new[] { value, value };
        }

        var sanitized = new FormattedLogValuesObfuscator(CreateObfuscator()).TryObfuscateValues(value);
        var pending = new Stack<object>();
        pending.Push(sanitized);
        var count = 0;
        while (pending.TryPop(out var current) && count < 1100)
        {
            count++;
            if (current is IEnumerable elements && current is not string)
            {
                foreach (var element in elements.Cast<object>())
                {
                    pending.Push(element);
                }
            }
            else
            {
                await Assert.That((string) current).DoesNotContain(Secret);
            }
        }

        // Up to one truncation marker per active collection may exceed the value budget.
        await Assert.That(count).IsLessThanOrEqualTo(1024 + 16);
    }

    [Test]
    public async Task Objects_AreReplacedWithSafeTextBeforeDestructuring()
    {
        object[] values =
        [
            new Credentials { Token = Secret },
            new FriendlyCredentials { Token = Secret },
            new { Token = Secret },
            new CredentialValue { Token = Secret },
            new KeyValuePair<string, string>("token", Secret),
        ];
        var obfuscator = new FormattedLogValuesObfuscator(CreateObfuscator());
        foreach (var value in values)
        {
            var state = new[] { new KeyValuePair<string, object?>("@Obj", value) };
            var sanitized = (IReadOnlyList<KeyValuePair<string, object?>>) obfuscator.TryObfuscateValues(state);
            await Assert.That(sanitized[0].Value).IsTypeOf<string>();
            await Assert.That((string) sanitized[0].Value!).DoesNotContain(Secret);
        }
    }

    [Test]
    public async Task Dictionary_DoesNotExposeRawKeysOrValues()
    {
        var dictionary = new Dictionary<string, object?> { [Secret] = new Credentials { Token = Secret } };
        var sanitized = new FormattedLogValuesObfuscator(CreateObfuscator()).TryObfuscateValues(dictionary);
        var elements = ((IEnumerable) sanitized).Cast<object?>().ToArray();
        await Assert.That(elements[0]).IsTypeOf<string>();
        await Assert.That((string) elements[0]!).DoesNotContain(Secret);
    }

    [Test]
    public async Task CollectionSnapshot_DoesNotExposeLaterMutations()
    {
        var values = new List<string> { "ordinary" };
        var sanitized = new FormattedLogValuesObfuscator(CreateObfuscator()).TryObfuscateValues(values);
        values[0] = Secret;
        var elements = ((IEnumerable) sanitized).Cast<object?>().ToArray();
        await Assert.That(elements[0]).IsEqualTo("ordinary");
    }

    [Test]
    public async Task Collections_AreNotInspectedWithoutRegisteredSecrets()
    {
        var values = new List<object> { new Credentials { Token = Secret } };
        var sanitized = new FormattedLogValuesObfuscator(CreateObfuscator(hasSecrets: false)).TryObfuscateValues(values);
        await Assert.That(sanitized).IsSameReferenceAs(values);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task CyclicAndUnboundedCollections_AreBounded(int elementKind)
    {
        var cycle = new List<object>();
        cycle.Add(cycle);
        var obfuscator = new FormattedLogValuesObfuscator(CreateObfuscator());
        var sanitizedCycle = obfuscator.TryObfuscateValues(cycle);
        var sanitizedInfinite = obfuscator.TryObfuscateValues(InfiniteValues(elementKind));
        var leaf = sanitizedCycle;
        var depth = 0;
        while (leaf is IEnumerable elements && leaf is not string && depth < 20)
        {
            leaf = elements.Cast<object>().Single();
            depth++;
        }

        await Assert.That(leaf).IsTypeOf<string>();
        await Assert.That(((IEnumerable) sanitizedInfinite).Cast<object?>().Count()).IsLessThan(2000);
    }

    [Test]
    public async Task ThrowingCollection_DoesNotReturnTheOriginalObject()
    {
        var values = ThrowingValues();
        var sanitized = new FormattedLogValuesObfuscator(CreateObfuscator()).TryObfuscateValues(values);
        await Assert.That(sanitized).IsTypeOf<string>();
        await Assert.That((string) sanitized).DoesNotContain(Secret);
    }

    private static IEnumerable<object?> InfiniteValues(int elementKind)
    {
        while (true)
        {
            yield return elementKind switch
            {
                0 => Secret,
                1 => new PreObfuscatedLogValue("already-masked"),
                _ => null,
            };
        }
    }

    private static IEnumerable<string> ThrowingValues()
    {
        yield return Secret;
        throw new InvalidOperationException(Secret);
    }

    private static IEnumerable<char> InfiniteCharacters()
    {
        while (true)
        {
            yield return 'x';
        }
    }

    private static IEnumerable<char> ThrowingCharacters()
    {
        yield return Secret[0];
        throw new InvalidOperationException(Secret);
    }

    private static SecretObfuscator CreateObfuscator(bool hasSecrets = true, SecretMaskingOptions? maskingOptions = null)
    {
        var snapshot = new SecretSnapshot(0, hasSecrets ? [Secret] : []);
        var provider = Mock.Of<ISecretProvider>(x => x.Version == 0 && x.GetSnapshot() == snapshot);
        return new SecretObfuscator(provider, Microsoft.Extensions.Options.Options.Create(maskingOptions ?? new SecretMaskingOptions()));
    }

    private struct CredentialValue
    {
        public string Token { get; init; }
    }

    private sealed class Credentials
    {
        public required string Token { get; init; }
    }

    private sealed class FriendlyCredentials
    {
        public required string Token { get; init; }

        public override string ToString() => "credentials";
    }
}
