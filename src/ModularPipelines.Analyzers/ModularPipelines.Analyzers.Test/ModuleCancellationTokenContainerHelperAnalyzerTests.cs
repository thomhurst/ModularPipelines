using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleCancellationTokenContainerHelperAnalyzerTests
{
    [TestMethod]
    [DataRow("var scratch = value; if (flag) goto Write; scratch = new CancellationToken[1]; Write: scratch[0] = default;", true)]
    [DataRow("var scratch = value; (scratch, _) = (value, 0); if (flag) goto Write; scratch = new CancellationToken[1]; Write: scratch[0] = default;", true)]
    [DataRow("var scratch = value; if (false) goto Write; scratch = new CancellationToken[1]; Write: scratch[0] = default;", false)]
    [DataRow("var scratch = value; if (flag) goto Reset; Reset: scratch = new CancellationToken[1]; scratch[0] = default;", false)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; int passes = 0; Again: alias[0] = default; alias = ref value; alias = ref scratch; if (passes++ == 0) goto Again;", false)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; int passes = 0; Again: alias[0] = default; alias = ref value; if (flag) goto Jump; alias = ref scratch; Jump: if (passes++ == 0) goto Again;", true)]
    [DataRow("var scratch = value; void Bind() { scratch = value; } void BindIndirect() { Bind(); } BindIndirect(); scratch = new CancellationToken[1]; scratch[0] = default;", false)]
    [DataRow("var scratch = value; Action bind = () => scratch = value; bind(); scratch = new CancellationToken[1]; scratch[0] = default;", false)]
    [DataRow("var scratch = value; void Bind() { scratch = value; } scratch = new CancellationToken[1]; if (flag) Bind(); scratch[0] = default;", true)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; var passes = 0; Again: alias[0] = default; if (false) alias = ref value; if (passes++ == 0) goto Again;", false)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; var passes = 0; Again: alias[0] = default; if (flag) alias = ref value; if (passes++ == 0) goto Again;", true)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; var passes = 0; Again: alias[0] = default; alias = ref value; if (passes++ == 0) goto Again;", true)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; var passes = 0; Again: alias = ref scratch; alias[0] = default; alias = ref value; if (passes++ == 0) goto Again;", false)]
    [DataRow("var scratch = value; void Bind() { scratch = value; } Bind(); scratch = new CancellationToken[1]; scratch[0] = default;", false)]
    [DataRow("var scratch = value; (scratch, _) = (value, 0); scratch = new CancellationToken[1]; scratch[0] = default;", false)]
    [DataRow("var scratch = value; scratch = new CancellationToken[1]; int passes = 0; Again: scratch[0] = default; if (passes++ == 0) goto Again;", false)]
    [DataRow("var scratch = value; scratch = new CancellationToken[1]; for (var i = 0; i < 2; i++) { scratch[0] = default; (scratch, _) = (value, 0); }", true)]
    [DataRow("var scratch = value; scratch = new CancellationToken[1]; Action bind = () => scratch = value; bind(); scratch[0] = default;", true)]
    [DataRow("var scratch = value; try { if (flag) throw new Exception(); scratch = new CancellationToken[1]; } catch (Exception) { } scratch[0] = default;", true)]
    [DataRow("var scratch = value; scratch = new CancellationToken[1]; void Bind() { scratch = value; } Bind(); scratch[0] = default;", true)]
    [DataRow("var scratch = value; scratch = new CancellationToken[1]; (scratch, _) = (value, 0); scratch[0] = default;", true)]
    [DataRow("var scratch = value; scratch = new CancellationToken[1]; var passes = 0; Again: scratch[0] = default; scratch = value; if (passes++ == 0) goto Again;", true)]
    [DataRow("var scratch = value; scratch = new CancellationToken[1]; void Bind() { scratch = value; } scratch[0] = default;", false)]
    [DataRow("var scratch = value; scratch = new CancellationToken[1]; void Bind() { scratch = value; } if (false) Bind(); scratch[0] = default;", false)]
    [DataRow("var scratch = value; scratch = new CancellationToken[1]; var passes = 0; Again: scratch = new CancellationToken[1]; scratch[0] = default; scratch = value; if (passes++ == 0) goto Again;", false)]
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
    [DataRow("_ = new Lazy<bool>(() => { ClearLocal(); return true; }).Value; void ClearLocal() { value[0] = default; }", true)]
    [DataRow("_ = new Lazy<bool>(() => { ClearIndirect(); return true; }).Value; void ClearIndirect() { ClearLocal(); } void ClearLocal() { value[0] = default; }", true)]
    [DataRow("Func<bool> clear = () => { ClearLocal(); return true; }; _ = new Lazy<bool>(clear).Value; void ClearLocal() { value[0] = default; }", true)]
    [DataRow("Func<bool> clear = () => { ClearLocal(); return true; }; void ClearLocal() { value[0] = default; }", false)]
    [DataRow("if (false) _ = new Lazy<bool>(() => { ClearLocal(); return true; }).Value; void ClearLocal() { value[0] = default; }", false)]
    [DataRow("var scratch = new CancellationToken[1]; ref var alias = ref value; alias = ref scratch;", false)]
    [DataRow("var scratch = new CancellationToken[1]; ref var alias = ref value; alias = ref scratch; alias = new CancellationToken[1];", false)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; alias = ref value; alias = new CancellationToken[1];", true)]
    [DataRow("var scratch = new CancellationToken[2]; ref var alias = ref value; scratch[alias[0].CanBeCanceled ? 0 : 1] = default;", false)]
    [DataRow("var scratch = new CancellationToken[1]; ref var alias = ref value; if (flag) alias = ref scratch; alias = new CancellationToken[1];", true)]
    [DataRow("ref var alias = ref value; alias = ref alias; alias[0] = default;", true)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; for (var i = 0; i < 2; i++) { alias[0] = default; alias = ref value; }", true)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; var i = 0; while (i++ < 2) { alias[0] = default; alias = ref value; }", true)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; var i = 0; do { alias[0] = default; alias = ref value; } while (i++ < 2);", true)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; for (var i = 0; i < 2; i++) { alias = new CancellationToken[1]; alias = ref value; }", true)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; for (var i = 0; i < 2; i++) { alias = ref scratch; alias[0] = default; alias = ref value; }", false)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; for (var i = 0; i < 2; i++) { alias[0] = default; if (false) alias = ref value; }", false)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; for (var i = 0; i < 2; i++) { ref CancellationToken[] alias = ref scratch; alias[0] = default; alias = ref value; }", false)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; for (var i = 0; i < 2; i++) { alias[0] = default; alias = ref value; alias = ref scratch; }", false)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; for (var i = 0; i < 2; i++) { alias[0] = default; alias = ref value; if (flag) alias = ref scratch; }", true)]
    [DataRow("CancellationToken[] s1 = new CancellationToken[1]; CancellationToken[] s2 = new CancellationToken[1]; ref CancellationToken[] a = ref s1; ref CancellationToken[] b = ref s2; for (var i = 0; i < 2; i++) { a[0] = default; a = ref (flag ? ref b : ref s1); b = ref (flag ? ref a : ref s2); }", false)]
    [DataRow("CancellationToken[] s1 = new CancellationToken[1]; CancellationToken[] s2 = new CancellationToken[1]; ref CancellationToken[] a = ref s1; ref CancellationToken[] b = ref s2; for (var i = 0; i < 2; i++) { a[0] = default; a = ref (flag ? ref b : ref value); b = ref (flag ? ref a : ref s2); }", true)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; for (var i = 0; i < 2; i++) { alias[0] = default; alias = ref value; if (i == 0) continue; alias = ref scratch; }", true)]
    [DataRow("_ = MakeScratch(); CancellationToken[] MakeScratch() { var scratch = new CancellationToken[1]; scratch[0] = default; return scratch; }", false)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; for (var i = 0; i < 2; i++) { alias[0] = default; alias = ref value; if (false) continue; alias = ref scratch; }", false)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; var i = 0; while (i++ < 2) { alias[0] = default; alias = ref value; if (i == 1) continue; alias = ref scratch; }", true)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; for (var i = 0; i < 2; i++) { alias[0] = default; alias = ref value; if (flag) break; alias = ref scratch; }", false)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; var i = 0; while (i++ < 2) { alias[0] = default; alias = ref value; if (flag) break; alias = ref scratch; }", false)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; var i = 0; do { alias[0] = default; alias = ref value; if (flag) break; alias = ref scratch; } while (i++ < 2);", false)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; for (var i = 0; i < 2; i++) { alias[0] = default; alias = ref value; switch (i) { case 0: break; } alias = ref scratch; }", false)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; for (var i = 0; i < 2; i++) { alias[0] = default; alias = ref value; if (flag) goto Done; alias = ref scratch; } Done: ;", false)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; for (var i = 0; i < 2; i++) { alias[0] = default; alias = ref value; if (flag) goto Next; alias = ref scratch; Next: ; }", true)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; for (var i = 0; i < 2; i++) { alias[0] = default; alias = ref value; if (flag) goto Reset; Reset: alias = ref scratch; }", false)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; for (var i = 0; i < 2; i++) { alias[0] = default; for (var j = 0; j < 2; j++) { alias = ref value; if (flag) break; alias = ref scratch; } }", true)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; for (var i = 0; i < 2; i++) { alias[0] = default; for (var j = 0; j < 2; j++) { alias = ref value; if (flag) continue; alias = ref scratch; } }", true)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; for (var i = 0; i < 2; i++) { alias[0] = default; switch (i) { case 0: alias = ref value; if (flag) goto case 1; alias = ref scratch; break; case 1: break; } }", true)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; for (var i = 0; i < 2; i++) { alias = ref value; alias[0] = default; if (flag) break; alias = ref scratch; }", true)]
    [DataRow("var scratch = value; scratch = new CancellationToken[1]; scratch[0] = default;", false)]
    [DataRow("var scratch = value; var old = scratch; scratch = new CancellationToken[1]; old[0] = default;", true)]
    [DataRow("var scratch = value; scratch = new CancellationToken[1]; var next = scratch; next[0] = default;", false)]
    [DataRow("var scratch = value; if (true) scratch = new CancellationToken[1]; scratch[0] = default;", false)]
    [DataRow("var scratch = value; if (flag) scratch = new CancellationToken[1]; scratch[0] = default;", true)]
    [DataRow("CancellationToken[] scratch = new CancellationToken[1]; ref CancellationToken[] alias = ref scratch; var passes = 0; Again: for (var i = 0; i < 2; i++) { alias[0] = default; alias = ref value; if (passes++ == 0) goto Again; alias = ref scratch; }", true)]
    [DataRow("var scratch = value; scratch = new CancellationToken[1]; for (var i = 0; i < 2; i++) { scratch[0] = default; scratch = value; }", true)]
    [DataRow("var scratch = value; for (var i = 0; i < 2; i++) { scratch = new CancellationToken[1]; scratch[0] = default; scratch = value; }", false)]
    [DataRow("var scratch = value; scratch = new CancellationToken[1]; for (var i = 0; i < 2; i++) { scratch[0] = default; }", false)]
    [DataRow("var scratch = value; scratch = new CancellationToken[1]; ref var alias = ref scratch; alias = value; scratch[0] = default;", true)]
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
    [DataRow("var scratch = tokens; try { if (context != null) throw new Exception(); scratch = new CancellationToken[1]; } catch (Exception) { } scratch[0] = default;", true)]
    [DataRow("var scratch = tokens; scratch = new CancellationToken[1]; void Bind() { scratch = tokens; } Bind(); scratch[0] = default;", true)]
    [DataRow("var scratch = tokens; scratch = new CancellationToken[1]; (scratch, _) = (tokens, 0); scratch[0] = default;", true)]
    [DataRow("var scratch = tokens; scratch = new CancellationToken[1]; var passes = 0; Again: scratch[0] = default; scratch = tokens; if (passes++ == 0) goto Again;", true)]
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
    [DataRow("CancellationToken[] alias; alias = (tokens = new[] { cancellationToken }); alias[0] = default;", true)]
    [DataRow("CancellationToken[] alias; alias = (tokens = new[] { cancellationToken }); var other = alias; other[0] = default;", true)]
    [DataRow("CancellationToken[] alias; alias = (tokens = new[] { cancellationToken }); var scratch = new CancellationToken[1]; scratch[0] = default;", false)]
    [DataRow("_ = new Lazy<bool>(() => { tokens[0] = default; return true; }).Value;", true)]
    [DataRow("Func<bool> clear = () => { tokens[0] = default; return true; }; _ = new Lazy<bool>(clear).Value;", true)]
    [DataRow("_ = new Lazy<bool>(() => { tokens[0] = default; return true; }).Value; tokens = new[] { cancellationToken };", false)]
    [DataRow("if (false) _ = new Lazy<bool>(() => { tokens[0] = default; return true; }).Value;", false)]
    [DataRow("ref var alias = ref tokens; alias = new CancellationToken[1];", true)]
    [DataRow("var scratch = new CancellationToken[1]; ref var alias = ref scratch; alias = new CancellationToken[1];", false)]
    [DataRow("ref var alias = ref tokens; alias = new CancellationToken[1]; tokens = new[] { cancellationToken };", false)]
    [DataRow("var scratch = new CancellationToken[1]; ref var alias = ref tokens; alias = ref scratch;", false)]
    [DataRow("var scratch = new CancellationToken[1]; ref var alias = ref tokens; alias = ref scratch; alias = new CancellationToken[1];", false)]
    [DataRow("var scratch = new CancellationToken[1]; ref var alias = ref scratch; alias = ref tokens; alias = new CancellationToken[1];", true)]
    [DataRow("var scratch = new CancellationToken[2]; ref var alias = ref tokens; scratch[alias[0].CanBeCanceled ? 0 : 1] = default;", false)]
    [DataRow("var scratch = new CancellationToken[1]; ref var alias = ref tokens; if (context != null) alias = ref scratch; alias = new CancellationToken[1];", true)]
    [DataRow("ref var alias = ref tokens; alias = ref alias; alias[0] = default;", true)]
    [DataRow("void ClearScratch(CancellationToken[] scratch) => scratch[0] = default; ClearScratch(new CancellationToken[1]);", false)]
    [DataRow("void ClearScratch(CancellationToken[] scratch) => scratch[0] = default; ClearScratch(tokens);", true)]
    [DataRow("void ClearScratch(CancellationToken[] scratch) { var alias = scratch; alias[0] = default; } ClearScratch(new CancellationToken[1]);", false)]
    [DataRow("_ = MakeScratch(); CancellationToken[] MakeScratch() { var scratch = new CancellationToken[1]; scratch[0] = default; return scratch; }", false)]
    [DataRow("_ = MakeTokens(); CancellationToken[] MakeTokens() { tokens[0] = default; return tokens; }", true)]
    [DataRow("var scratch = tokens; scratch = new CancellationToken[1]; scratch[0] = default;", false)]
    [DataRow("var scratch = tokens; var old = scratch; scratch = new CancellationToken[1]; old[0] = default;", true)]
    [DataRow("var scratch = tokens; scratch = new CancellationToken[1]; var next = scratch; next[0] = default;", false)]
    [DataRow("var scratch = tokens; if (true) scratch = new CancellationToken[1]; scratch[0] = default;", false)]
    [DataRow("var scratch = tokens; if (context != null) scratch = new CancellationToken[1]; scratch[0] = default;", true)]
    [DataRow("var scratch = tokens; scratch = new CancellationToken[1]; for (var i = 0; i < 2; i++) { scratch[0] = default; scratch = tokens; }", true)]
    [DataRow("var scratch = tokens; for (var i = 0; i < 2; i++) { scratch = new CancellationToken[1]; scratch[0] = default; scratch = tokens; }", false)]
    [DataRow("var scratch = tokens; scratch = new CancellationToken[1]; for (var i = 0; i < 2; i++) { scratch[0] = default; }", false)]
    [DataRow("var scratch = tokens; scratch = new[] { cancellationToken }; scratch[0] = default; tokens = Forward(scratch);", true)]
    [DataRow("var scratch = tokens; scratch = new CancellationToken[1]; ref var alias = ref scratch; alias = tokens; scratch[0] = default;", true)]
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

    [TestMethod]
    [DataRow("var scratch = tokens; scratch = new CancellationToken[1]; scratch[0] = default;", false, false)]
    [DataRow("tokens[0] = default;", true, false)]
    [DataRow("var scratch = tokens; scratch = new CancellationToken[1]; scratch = tokens; scratch[0] = default;", true, false)]
    // Returning through another call remains a conservative container escape.
    [DataRow("var scratch = tokens; scratch = new CancellationToken[1]; scratch[0] = default;", true, true)]
    [DataRow("tokens[0] = default;", true, true)]
    [DataRow("var scratch = tokens; scratch = new CancellationToken[1]; scratch = tokens; scratch[0] = default;", true, true)]
    public async Task Tracks_Fresh_Returned_Container_Storage(string body, bool warns, bool forwards)
    {
        var call = warns ? "{|#0:Task.Delay(1, linked.Token)|}" : "Task.Delay(1, linked.Token)";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeaderWithExtensions}}
            public class BuildModule : Module<List<string>>
            {
                protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                {
                    using var linked = CancellationTokenSource.CreateLinkedTokenSource(Forward(cancellationToken));
                    await {{call}};
                    return new List<string>();
                }

                private static CancellationToken[] Forward(CancellationToken token)
                {
                    var tokens = new[] { token };
                    {{body}}
                    return {{(forwards ? "Identity(tokens)" : "tokens")}};
                }

                private static CancellationToken[] Identity(CancellationToken[] value) => value;
            }
            """;
        await VerifyCS.VerifyAnalyzerAsync(source, warns
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }

    [TestMethod]
    [DataRow("Forward(tokens, tokens[0] = default)", true)]
    [DataRow("Forward(tokens, Clear())", true)]
    [DataRow("Forward(tokens, ClearIndirect())", true)]
    [DataRow("Forward(tokens, new Lazy<CancellationToken>(() => Clear()).Value)", true)]
    [DataRow("Forward(tokens, default)", false)]
    [DataRow("Forward(tokens, scratch[0] = default)", false)]
    [DataRow("Forward(tokens, context != null ? tokens[0] = default : default)", true)]
    [DataRow("Forward(tokens, false ? tokens[0] = default : default)", false)]
    public async Task Tracks_Container_Argument_Evaluation(string argument, bool warns)
    {
        var call = warns ? "{|#0:Task.Delay(1, linked.Token)|}" : "Task.Delay(1, linked.Token)";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeaderWithExtensions}}
            public class BuildModule : Module<List<string>>
            {
                protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                {
                    var tokens = new[] { cancellationToken };
                    var scratch = new CancellationToken[1];
                    using var linked = CancellationTokenSource.CreateLinkedTokenSource({{argument}});
                    await {{call}};
                    return new List<string>();
                    CancellationToken Clear() { tokens[0] = default; return default; }
                    CancellationToken ClearIndirect() => Clear();
                }

                private static CancellationToken[] Forward(CancellationToken[] value, CancellationToken ignored) => value;
            }
            """;
        await VerifyCS.VerifyAnalyzerAsync(source, warns
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }

    [TestMethod]
    [DataRow("await WaitAsync(CancellationTokenSource.CreateLinkedTokenSource(Forward(tokens)).Token, tokens[0] = default);", false)]
    [DataRow("await WaitAsync(CancellationTokenSource.CreateLinkedTokenSource(tokens).Token, tokens[0] = default);", false)]
    [DataRow("await Task.WhenAll(Task.Delay(1, CancellationTokenSource.CreateLinkedTokenSource(tokens).Token), ClearAsync());", false)]
    [DataRow("await Task.WhenAll(Task.Delay(1, CancellationTokenSource.CreateLinkedTokenSource(Forward(tokens)).Token), ClearAsync());", false)]
    [DataRow("await WaitAsync(CancellationTokenSource.CreateLinkedTokenSource(Forward(tokens, tokens[0] = default)).Token, default);", true)]
    public async Task Stops_Mutation_Window_At_Cancellation_Capture(string statement, bool warns)
    {
        if (warns)
        {
            statement = statement.Replace("await WaitAsync(", "await {|#0:WaitAsync(").Replace(", default);", ", default)|};");
        }

        var source = $$"""
            {{TestSourceConstants.StandardModuleHeaderWithExtensions}}
            public class BuildModule : Module<List<string>>
            {
                protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                {
                    var tokens = new[] { cancellationToken };
                    {{statement}}
                    return new List<string>();
                    Task ClearAsync() { tokens[0] = default; return Task.CompletedTask; }
                }

                private static Task WaitAsync(CancellationToken cancellationToken, CancellationToken ignored) => Task.CompletedTask;
                private static CancellationToken[] Forward(CancellationToken[] value, CancellationToken ignored = default) => value;
            }
            """;
        await VerifyCS.VerifyAnalyzerAsync(source, warns
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("WaitAsync")]
            : []);
    }
}
