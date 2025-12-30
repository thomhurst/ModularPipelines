using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

/// <summary>
/// Consolidated tests for encoding and hashing operations using TUnit's data-driven testing.
/// This replaces the individual Base64Tests, HexTests, Md5Tests, Sha1Tests, Sha256Tests,
/// Sha384Tests, and Sha512Tests classes to eliminate DRY violations.
/// </summary>
public class EncodingTests : TestBase
{
    private const string TestInput = "Foo bar!";

    #region Test Data

    /// <summary>
    /// Provides test data for bidirectional encoding tests (Base64 and Hex).
    /// Each item contains: encoding name, encode function, decode function, encoded output.
    /// </summary>
    public static IEnumerable<(string EncodingName, Func<IModuleContext, string> Encode, Func<IModuleContext, string> Decode, string EncodedValue)> BidirectionalEncodingData()
    {
        yield return ("Base64",
            ctx => ctx.Base64.ToBase64String(TestInput),
            ctx => ctx.Base64.FromBase64String("Rm9vIGJhciE="),
            "Rm9vIGJhciE=");

        yield return ("Hex",
            ctx => ctx.Hex.ToHex(TestInput),
            ctx => ctx.Hex.FromHex("466f6f2062617221"),
            "466f6f2062617221");
    }

    /// <summary>
    /// Provides test data for one-way hash tests (MD5, SHA1, SHA256, SHA384, SHA512).
    /// Each item contains: hash name, hash function, expected hash output.
    /// </summary>
    public static IEnumerable<(string HashName, Func<IModuleContext, string> HashFunc, string ExpectedHash)> HashData()
    {
        yield return ("Md5",
            ctx => ctx.Hasher.Md5(TestInput),
            "b9c291e3274aa5c8010a7c5c22a4e6dd");

        yield return ("Sha1",
            ctx => ctx.Hasher.Sha1(TestInput),
            "cc3626c5ad2e3aff0779dc63e80555c463fd99dc");

        yield return ("Sha256",
            ctx => ctx.Hasher.Sha256(TestInput),
            "d80c14a132a9ae008c78db4ee4cbc46b015b5e0f018f6b0a3e4ea5041176b852");

        yield return ("Sha384",
            ctx => ctx.Hasher.Sha384(TestInput),
            "bb338a277da65d5663467d5fd98aa67349506150cd1287597b0eaa0f0988d2b22c33504fd85dd0b8c99ce8cc50666f88");

        yield return ("Sha512",
            ctx => ctx.Hasher.Sha512(TestInput),
            "e399b0584705f5f229a4398baa31c4b7cc820ac208327d26e66f0668288536981c3460a7ea92ef6be488ce30ff5b6a991babfe24833094eba3226cea5c14162c");
    }

    #endregion

    #region Module Definitions

    /// <summary>
    /// Generic module that executes an encoding/hashing function via a factory delegate.
    /// </summary>
    private class EncodingModule : Module<string>
    {
        private readonly Func<IModuleContext, string> _operation;

        public EncodingModule(Func<IModuleContext, string> operation)
        {
            _operation = operation;
        }

        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return _operation(context);
        }
    }

    // Concrete module classes for each encoding/hash operation
    // These are required because RunModule<T> needs concrete types

    private class ToBase64Module : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Base64.ToBase64String(TestInput);
        }
    }

    private class FromBase64Module : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Base64.FromBase64String("Rm9vIGJhciE=");
        }
    }

    private class ToHexModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hex.ToHex(TestInput);
        }
    }

    private class FromHexModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hex.FromHex("466f6f2062617221");
        }
    }

    private class Md5Module : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hasher.Md5(TestInput);
        }
    }

    private class Sha1Module : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hasher.Sha1(TestInput);
        }
    }

    private class Sha256Module : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hasher.Sha256(TestInput);
        }
    }

    private class Sha384Module : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hasher.Sha384(TestInput);
        }
    }

    private class Sha512Module : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hasher.Sha512(TestInput);
        }
    }

    #endregion

    #region Base64 Tests

    [Test]
    [DisplayName("Base64: ToBase64String does not error")]
    public async Task To_Base64_Has_Not_Errored()
    {
        var moduleResult = await await RunModule<ToBase64Module>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    [Test]
    [DisplayName("Base64: ToBase64String produces correct output")]
    public async Task To_Base64_Output_Is_Correct()
    {
        var moduleResult = await await RunModule<ToBase64Module>();
        await Assert.That(moduleResult.Value).IsEqualTo("Rm9vIGJhciE=");
    }

    [Test]
    [DisplayName("Base64: FromBase64String does not error")]
    public async Task From_Base64_Has_Not_Errored()
    {
        var moduleResult = await await RunModule<FromBase64Module>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    [Test]
    [DisplayName("Base64: FromBase64String produces correct output")]
    public async Task From_Base64_Output_Is_Correct()
    {
        var moduleResult = await await RunModule<FromBase64Module>();
        await Assert.That(moduleResult.Value).IsEqualTo(TestInput);
    }

    #endregion

    #region Hex Tests

    [Test]
    [DisplayName("Hex: ToHex does not error")]
    public async Task To_Hex_Has_Not_Errored()
    {
        var moduleResult = await await RunModule<ToHexModule>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    [Test]
    [DisplayName("Hex: ToHex produces correct output")]
    public async Task To_Hex_Output_Is_Correct()
    {
        var moduleResult = await await RunModule<ToHexModule>();
        await Assert.That(moduleResult.Value).IsEqualTo("466f6f2062617221");
    }

    [Test]
    [DisplayName("Hex: FromHex does not error")]
    public async Task From_Hex_Has_Not_Errored()
    {
        var moduleResult = await await RunModule<FromHexModule>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    [Test]
    [DisplayName("Hex: FromHex produces correct output")]
    public async Task From_Hex_Output_Is_Correct()
    {
        var moduleResult = await await RunModule<FromHexModule>();
        await Assert.That(moduleResult.Value).IsEqualTo(TestInput);
    }

    #endregion

    #region Hash Tests

    [Test]
    [DisplayName("Md5: Hash does not error")]
    public async Task Md5_Has_Not_Errored()
    {
        var moduleResult = await await RunModule<Md5Module>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    [Test]
    [DisplayName("Md5: Hash produces correct output")]
    public async Task Md5_Output_Is_Correct()
    {
        var moduleResult = await await RunModule<Md5Module>();
        await Assert.That(moduleResult.Value).IsEqualTo("b9c291e3274aa5c8010a7c5c22a4e6dd");
    }

    [Test]
    [DisplayName("Sha1: Hash does not error")]
    public async Task Sha1_Has_Not_Errored()
    {
        var moduleResult = await await RunModule<Sha1Module>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    [Test]
    [DisplayName("Sha1: Hash produces correct output")]
    public async Task Sha1_Output_Is_Correct()
    {
        var moduleResult = await await RunModule<Sha1Module>();
        await Assert.That(moduleResult.Value).IsEqualTo("cc3626c5ad2e3aff0779dc63e80555c463fd99dc");
    }

    [Test]
    [DisplayName("Sha256: Hash does not error")]
    public async Task Sha256_Has_Not_Errored()
    {
        var moduleResult = await await RunModule<Sha256Module>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    [Test]
    [DisplayName("Sha256: Hash produces correct output")]
    public async Task Sha256_Output_Is_Correct()
    {
        var moduleResult = await await RunModule<Sha256Module>();
        await Assert.That(moduleResult.Value).IsEqualTo("d80c14a132a9ae008c78db4ee4cbc46b015b5e0f018f6b0a3e4ea5041176b852");
    }

    [Test]
    [DisplayName("Sha384: Hash does not error")]
    public async Task Sha384_Has_Not_Errored()
    {
        var moduleResult = await await RunModule<Sha384Module>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    [Test]
    [DisplayName("Sha384: Hash produces correct output")]
    public async Task Sha384_Output_Is_Correct()
    {
        var moduleResult = await await RunModule<Sha384Module>();
        await Assert.That(moduleResult.Value).IsEqualTo("bb338a277da65d5663467d5fd98aa67349506150cd1287597b0eaa0f0988d2b22c33504fd85dd0b8c99ce8cc50666f88");
    }

    [Test]
    [DisplayName("Sha512: Hash does not error")]
    public async Task Sha512_Has_Not_Errored()
    {
        var moduleResult = await await RunModule<Sha512Module>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    [Test]
    [DisplayName("Sha512: Hash produces correct output")]
    public async Task Sha512_Output_Is_Correct()
    {
        var moduleResult = await await RunModule<Sha512Module>();
        await Assert.That(moduleResult.Value).IsEqualTo("e399b0584705f5f229a4398baa31c4b7cc820ac208327d26e66f0668288536981c3460a7ea92ef6be488ce30ff5b6a991babfe24833094eba3226cea5c14162c");
    }

    #endregion
}
