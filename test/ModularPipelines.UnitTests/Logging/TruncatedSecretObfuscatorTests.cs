using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;
using ModularPipelines.Secrets;
using Moq;

namespace ModularPipelines.UnitTests.Logging;

public class TruncatedSecretObfuscatorTests
{
    [Test]
    [Arguments("safe-abc", false, true, "safe-**********")]
    [Arguments("def-safe", true, false, "**********-safe")]
    [Arguments("safe-a", false, true, "safe-**********")]
    [Arguments("f-safe", true, false, "**********-safe")]
    [Arguments("safe-abc", true, false, "safe-abc")]
    [Arguments("def-safe", false, true, "def-safe")]
    [Arguments("abc", false, true, "**********")]
    [Arguments("def", true, false, "**********")]
    [Arguments("safe-abcdef-abc", false, true, "safe-**********-**********")]
    [Arguments("abc-safe-abc", false, true, "abc-safe-**********")]
    [Arguments("abcdef", false, false, "**********")]
    [Arguments("", true, true, "")]
    public async Task Masks_Only_Complete_Secrets_And_Truncated_Edges(
        string input, bool truncatedStart, bool truncatedEnd, string expected)
    {
        var obfuscator = CreateObfuscator(["abcdef"]);

        await Assert.That(obfuscator.ObfuscateTruncated(input, null, truncatedStart, truncatedEnd)).IsEqualTo(expected);
    }

    [Test]
    [Arguments("safe-abcd", false, true, "safe-**********")]
    [Arguments("defg-safe", true, false, "**********-safe")]
    public async Task Masks_Complete_Secrets_Overlapping_Boundary_Fragments(
        string input, bool truncatedStart, bool truncatedEnd, string expected)
    {
        var obfuscator = CreateObfuscator(["abcd", "defg"]);

        await Assert.That(obfuscator.ObfuscateTruncated(input, null, truncatedStart, truncatedEnd)).IsEqualTo(expected);
    }

    [Test]
    public async Task Merges_Overlapping_Fragments_On_Both_Edges()
    {
        var obfuscator = CreateObfuscator(["abcabca"]);

        await Assert.That(obfuscator.ObfuscateTruncated("cabc", null, true, true)).IsEqualTo("**********");
    }

    [Test]
    [Arguments(true, "safe-[HIDDEN]")]
    [Arguments(false, "safe-AbC")]
    public async Task Respects_Case_Sensitivity_And_Custom_Mask(bool caseInsensitive, string expected)
    {
        var obfuscator = CreateObfuscator(["abcdef"], new SecretMaskingOptions
        {
            CaseInsensitive = caseInsensitive,
            MaskValue = "[HIDDEN]",
        });

        await Assert.That(obfuscator.ObfuscateTruncated("safe-AbC", null, false, true)).IsEqualTo(expected);
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    public async Task Uses_Default_Mask_When_Custom_Mask_Is_Blank(string mask)
    {
        var obfuscator = CreateObfuscator(["abcdef"], new SecretMaskingOptions { MaskValue = mask });

        await Assert.That(obfuscator.ObfuscateTruncated("safe-abc", null, false, true)).IsEqualTo("safe-**********");
    }

    [Test]
    public async Task Uses_Safe_Fallback_When_Mask_Contains_Secret()
    {
        var obfuscator = CreateObfuscator(["abcdef"], new SecretMaskingOptions { MaskValue = "abcdef" });

        await Assert.That(obfuscator.ObfuscateTruncated("safe-abc", null, false, true)).IsEqualTo("safe-[MASKED]");
    }

    [Test]
    public async Task Masks_Encoded_Secrets_And_Option_Secrets()
    {
        const string secret = "p@ss word\"value";
        var options = new SecretOptions { Value = secret };
        var obfuscator = CreateObfuscator([]);

        foreach (var pattern in SecretMaskingPatternGenerator.Generate(secret))
        {
            await Assert.That(obfuscator.ObfuscateTruncated(pattern[..^1], options, false, true)).IsEqualTo("**********");
            await Assert.That(obfuscator.ObfuscateTruncated(pattern[1..], options, true, false)).IsEqualTo("**********");
        }
    }

    [Test]
    public async Task Applies_Minimum_Length_To_Secrets_But_Masks_One_Character_Fragments()
    {
        var obfuscator = CreateObfuscator(["xyz", "long-secret"], new SecretMaskingOptions { MinimumSecretLength = 8 });

        await Assert.That(obfuscator.ObfuscateTruncated("safe-xy", null, false, true)).IsEqualTo("safe-xy");
        await Assert.That(obfuscator.ObfuscateTruncated("safe-l", null, false, true)).IsEqualTo("safe-**********");
    }

    private static SecretObfuscator CreateObfuscator(string[] secrets, SecretMaskingOptions? options = null)
    {
        var maskingOptions = Microsoft.Extensions.Options.Options.Create(options ?? new SecretMaskingOptions());
        var optionsProvider = new Mock<IOptionsProvider>();
        optionsProvider.Setup(provider => provider.GetOptions()).Returns([]);
        var provider = new SecretProvider(
            optionsProvider.Object,
            Mock.Of<IBuildSystemSecretMasker>(),
            maskingOptions,
            Mock.Of<ILogger<SecretProvider>>());
        provider.AddSecrets(secrets);
        return new SecretObfuscator(provider, maskingOptions);
    }

    private sealed class SecretOptions
    {
        [SecretValue]
        public required string Value { get; init; }
    }
}
