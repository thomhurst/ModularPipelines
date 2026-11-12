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
    [DataRow("var (alias, scratch) = flag ? (value, new CancellationToken[1]) : (value, new CancellationToken[1]); alias[0] = default;", true)]
    [DataRow("var (alias, scratch) = flag switch { true => (value, new CancellationToken[1]), false => (value, new CancellationToken[1]) }; alias[0] = default;", true)]
    [DataRow("value = new CancellationToken[1];", true)]
    [DataRow("var scratch = new CancellationToken[2]; scratch[value[0].CanBeCanceled ? 0 : 1] = default;", false)]
    [DataRow("if (false) value[0] = default;", false)]
    [DataRow("var scratch = new CancellationToken[1]; if (false) scratch = value; scratch[0] = default;", false)]
    [DataRow("Action clear = () => value[0] = default;", false)]
    [DataRow("void ClearLocal() { value[0] = default; }", false)]
    [DataRow("Action clear = () => value[0] = default; clear();", true)]
    [DataRow("void ClearLocal() { value[0] = default; } ClearLocal();", true)]
    [DataRow("_ = new Lazy<bool>(() => { value[0] = default; return true; }).Value;", true)]
    [DataRow("Func<bool> clear = () => { value[0] = default; return true; }; _ = new Lazy<bool>(clear).Value;", true)]
    [DataRow("var scratch = value; var fresh = (scratch = new CancellationToken[1]); fresh[0] = default;", false)]
    [DataRow("ref var alias = ref value; alias = new CancellationToken[1];", true)]
    [DataRow("var scratch = new CancellationToken[1]; ref var alias = ref scratch; alias = new CancellationToken[1];", false)]
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
    [DataRow("var scratch = new CancellationToken[1]; ref var token = ref scratch[0]; token = ref value[0]; token = default;", true)]
    [DataRow("var scratch = new CancellationToken[1]; (value[0], scratch[0]) = (default(CancellationToken), default(CancellationToken));", true)]
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

    [TestMethod]
    [DataRow("tokens[0] = default; tokens = [cancellationToken];", false)]
    [DataRow("var old = tokens; tokens = [cancellationToken]; old[0] = default;", false)]
    [DataRow("tokens = [cancellationToken]; tokens[0] = default;", true)]
    [DataRow("tokens[0] = default; tokens = new[] { cancellationToken };", false)]
    [DataRow("var old = tokens; tokens = new[] { cancellationToken }; old[0] = default;", false)]
    [DataRow("tokens[0] = default; tokens = tokens;", true)]
    [DataRow("tokens[0] = default; if (context != null) tokens = new[] { cancellationToken };", true)]
    [DataRow("Action clear = () => tokens[0] = default;", false)]
    [DataRow("void ClearLocal() { tokens[0] = default; }", false)]
    [DataRow("Action clear = () => tokens[0] = default; clear();", true)]
    [DataRow("Action clear = () => tokens[0] = default; tokens = new[] { cancellationToken }; clear();", true)]
    [DataRow("void ClearLocal() { tokens[0] = default; } ClearLocal();", true)]
    [DataRow("var alias = (tokens = new[] { cancellationToken }); alias[0] = default;", true)]
    [DataRow("ReplaceAfter();", true)]
    [DataRow("ReplaceIndirect();", true)]
    [DataRow("if (false) ReplaceAfter();", false)]
    [DataRow("ReplaceAfter(); tokens = new[] { cancellationToken };", false)]
    [DataRow("void ReplaceTuple() { (tokens, _) = (new CancellationToken[1], 0); } ReplaceTuple();", true)]
    [DataRow("var alias = tokens; void ReplaceAlias() { alias = new CancellationToken[1]; } ReplaceAlias();", false)]
    [DataRow("ClearAfter();", true)]
    [DataRow("ClearIndirect();", true)]
    [DataRow("if (false) ClearAfter();", false)]
    [DataRow("ClearAfter(); tokens = new[] { cancellationToken };", false)]
    [DataRow("tokens[0] = default;", true)]
    [DataRow("var alias = tokens; alias[0] = default;", true)]
    [DataRow("Clear(tokens);", true)]
    [DataRow("if (false) tokens[0] = default;", false)]
    [DataRow("var scratch = new CancellationToken[1]; scratch[0] = default;", false)]
    [DataRow("", false)]
    public async Task Tracks_Caller_Container_Storage(string body, bool warns)
    {
        var call = warns ? "{|#0:Task.Delay(1, linked.Token)|}" : "Task.Delay(1, linked.Token)";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeaderWithExtensions}}
            public class BuildModule : Module<List<string>>
            {
                protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                {
                    var tokens = new[] { cancellationToken };
                    {{body}}
                    using var linked = CancellationTokenSource.CreateLinkedTokenSource(Forward(tokens));
                    await {{call}};
                    return new List<string>();
                    void ClearAfter() { tokens[0] = default; }
                    void ClearIndirect() { ClearAfter(); }
                    void ReplaceAfter() { tokens = new CancellationToken[1]; }
                    void ReplaceIndirect() { ReplaceAfter(); }
                }

                private static void Clear(CancellationToken[] value) => value[0] = default;
                private static CancellationToken[] Forward(CancellationToken[] value) => value;
            }
            """;
        await VerifyCS.VerifyAnalyzerAsync(source, warns
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }
}
