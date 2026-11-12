using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleCancellationTokenContainerHelperAnalyzerTests
{
    [TestMethod]
    [DataRow("var alias = flag switch { true => value, false => value }; alias[0] = default;", true)]
    [DataRow("var (scratch, original) = (new CancellationToken[1], value); scratch[0] = default;", false)]
    [DataRow("Span<CancellationToken> span = value; span[0] = default;", true)]
    [DataRow("var scratch = new CancellationToken[1]; var alias = (scratch = value); alias[0] = default;", true)]
    [DataRow("", false)]
    [DataRow("var scratch = new CancellationToken[1]; scratch[0] = default;", false)]
    [DataRow("var scratch = new CancellationToken[value.Length]; scratch[0] = default;", false)]
    [DataRow("var scratch = new CancellationToken[1]; var index = value.Length - 1; scratch[index] = default;", false)]
    [DataRow("var (scratch, original) = (new CancellationToken[1], value); original[0] = default;", true)]
    [DataRow("var ((scratch, original), other) = ((new CancellationToken[1], value), new CancellationToken[1]); scratch[0] = default; other[0] = default;", false)]
    [DataRow("var ((scratch, original), other) = ((new CancellationToken[1], value), new CancellationToken[1]); original[0] = default;", true)]
    [DataRow("CancellationToken[] scratch, original; (scratch, original) = (new CancellationToken[1], value); scratch[0] = default;", false)]
    [DataRow("CancellationToken[] scratch, original; (scratch, original) = (new CancellationToken[1], value); original[0] = default;", true)]
    [DataRow("var scratch = new CancellationToken[1]; var alias = flag switch { true => scratch, false => scratch }; alias[0] = default;", false)]
    [DataRow("var scratch = new CancellationToken[1]; var alias = (scratch = new CancellationToken[1]); alias[0] = default;", false)]
    [DataRow("Span<CancellationToken> span = new CancellationToken[1]; span[0] = default;", false)]
    [DataRow("var alias = value; alias[0] = default;", true)]
    [DataRow("ref var token = ref value[0]; token = default;", true)]
    [DataRow("Clear(value);", true)]
    [DataRow("object erased = value;", true)]
    [DataRow("Storage = value;", true)]
    [DataRow("Property = value;", true)]
    [DataRow("ClearToken(ref value[0]);", true)]
    [DataRow("var pair = (new CancellationToken[1], value); var (scratch, original) = pair; scratch[0] = default;", false)]
    [DataRow("var pair = (new CancellationToken[1], value); var (scratch, original) = pair; original[0] = default;", true)]
    [DataRow("ClearOut(out value[0]);", true)]
    public async Task Tracks_Container_Helper_Storage(string body, bool warns)
    {
        var call = warns ? "{|#0:Task.Delay(1, linked.Token)|}" : "Task.Delay(1, linked.Token)";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeaderWithExtensions}}
            public class BuildModule : Module<List<string>>
            {
                protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                {
                    using var linked = CancellationTokenSource.CreateLinkedTokenSource(Forward(new[] { cancellationToken }, true));
                    await {{call}};
                    return new List<string>();
                }

                private static CancellationToken[] Storage = new CancellationToken[1];
                private static CancellationToken[] Property { get; set; } = new CancellationToken[1];
                private static void Clear(CancellationToken[] value) => value[0] = default;
                private static void ClearToken(ref CancellationToken value) => value = default;
                private static void ClearOut(out CancellationToken value) => value = default;

                private static CancellationToken[] Forward(CancellationToken[] value, bool flag)
                {
                    {{body}}
                    return value;
                }
            }
            """;
        await VerifyCS.VerifyAnalyzerAsync(source, warns
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }
}
