using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleHelperRefReachabilityAnalyzerTests
{
    [TestMethod]
    [DataRow("var saved = first; for (var i = 0; i < 2; i++) { if (Flag) { second = default; goto Done; } else { return first; } } Done: return saved;", false)]
    [DataRow("var saved = first; for (var i = 0; i < 2; i++) { if (Flag) { second = default; if (Flag) goto Done; } else { return first; } } Done: return saved;", true)]
    [DataRow("var saved = first; for (var i = 0; i < 2; i++) { if (Flag) { second = default; if (Flag) continue; goto Done; } else { return first; } } Done: return saved;", true)]
    [DataRow("var saved = first; for (var i = 0; i < 2; i++) { if (Flag) { second = default; if (Flag) continue; break; } else { return first; } } return saved;", true)]
    [DataRow("var saved = first; for (var i = 0; i < 2; i++) { if (Flag) { second = default; if (Flag) goto Next; break; } else { return first; } Next: continue; } return saved;", true)]
    [DataRow("var saved = first; for (var i = 0; i < 2; i++) { if (Flag) { second = default; break; } else { return first; } } return saved;", false)]
    [DataRow("var saved = first; for (var i = 0; i < 2; i++) { if (Flag) { second = default; continue; } else { return first; } } return saved;", true)]
    [DataRow("var saved = first; for (var i = 0; i < 2; i++) { if (Flag) { second = default; if (Flag) break; } else { return first; } } return saved;", true)]
    [DataRow("var saved = first; for (var i = 0; i < 2; i++) { if (Flag) { second = default; if (Flag) break; else break; } else { return first; } } return saved;", false)]
    [DataRow("var saved = first; for (var j = 0; j < 2; j++) { for (var i = 0; i < 2; i++) { if (Flag) { second = default; break; } else { return first; } } } return saved;", true)]
    [DataRow("goto Clear; Again: return first; Clear: second = default; goto Again;", true)]
    [DataRow("bool done = false; Again: if (done) return first; second = default; done = true; goto Again;", true)]
    [DataRow("bool done = false; Again: if (done) return first; second = first; done = true; goto Again;", false)]
    [DataRow("var saved = first; Again: if (Flag) return saved; second = default; goto Again;", false)]
    [DataRow("if (Flag) { second = default; throw new System.InvalidOperationException(); } return first;", false)]
    [DataRow("if (Flag) { second = default; if (Flag) throw new System.InvalidOperationException(); else return default; } return first;", true)]
    [DataRow("try { second = default; throw new System.InvalidOperationException(); } catch { return first; }", true)]
    [DataRow("try { second = default; throw new System.InvalidOperationException(); } catch { } return first;", true)]
    [DataRow("var saved = first; try { second = default; throw new System.InvalidOperationException(); } catch { return saved; }", false)]
    [DataRow("if (Flag) { try { second = default; throw new System.InvalidOperationException(); } catch { throw; } } return first;", false)]
    [DataRow("try { try { second = default; throw new System.InvalidOperationException(); } catch { throw; } } catch { return first; }", true)]
    [DataRow("var saved = first; if (Flag) { ref CancellationToken alias = ref saved; alias = default; return first; } second = saved; return first;", false)]
    [DataRow("var saved = first; if (Flag) { ref CancellationToken alias = ref saved; alias = default; } second = saved; return first;", true)]
    [DataRow("CancellationToken saved = first; try { second = default; throw new System.InvalidOperationException(); } catch (System.Exception) when ((saved = first) == default) { return saved; }", true)]
    [DataRow("CancellationToken saved = first; try { second = default; throw new System.InvalidOperationException(); } catch (System.Exception) when (saved == default) { return saved; }", false)]
    [DataRow("var saved = first; if (Flag) { second = default; } else { return first; } return saved;", false)]
    [DataRow("var saved = first; if (Flag) { second = default; return first; } else { return saved; }", true)]
    [DataRow("var stored = first; ref CancellationToken alias = ref stored; for (var i = 0; i < 2; i++) { second = stored; alias = default; } return first;", true)]
    [DataRow("var stored = first; ref CancellationToken alias = ref stored; for (var i = 0; i < 2; i++) { stored = first; second = stored; alias = default; } return first;", false)]
    [DataRow("for (var i = 0; i < 2; i++) { var saved = first; second = default; return saved; } return first;", false)]
    [DataRow("for (var i = 0; i < 2; i++) { var saved = first; second = default; if (Flag) return saved; else return saved; } return first;", false)]
    [DataRow("for (var i = 0; i < 2; i++) { var saved = first; second = default; if (Flag) return saved; } return first;", true)]
    [DataRow("for (var i = 0; i < 2; i++) { var saved = first; second = default; if (Flag) continue; return saved; } return first;", true)]
    [DataRow("var saved = first; switch (Flag) { case true: second = default; break; default: return first; } return saved;", false)]
    [DataRow("var saved = first; switch (Flag) { case true: second = default; goto default; default: return first; } return saved;", true)]
    [DataRow("var saved = first; switch (Count) { case 0: second = default; goto case 1; case 1: goto default; default: return first; } return saved;", true)]
    [DataRow("var saved = first; for (var i = 0; i < 2; i++) { switch (i) { case 0: second = default; break; default: return first; } } return saved;", true)]
    [DataRow("var saved = first; switch (Count) { case 0: second = default; goto case 1; case 1: return saved; default: return first; }", false)]
    [DataRow("var stored = first; ref CancellationToken alias = ref stored; bool done = false; Again: stored = first; second = stored; alias = default; if (!done) { done = true; goto Again; } return first;", false)]
    [DataRow("var stored = first; ref CancellationToken alias = ref stored; bool done = false; Again: if (!done) stored = first; second = stored; alias = default; if (!done) { done = true; goto Again; } return first;", true)]
    [DataRow("var stored = first; ref CancellationToken alias = ref stored; bool done = false; stored = first; Again: second = stored; alias = default; if (!done) { done = true; goto Again; } return first;", true)]
    [DataRow("var stored = first; ref CancellationToken alias = ref stored; bool done = false; Again: stored = first; Copy: second = stored; alias = default; if (!done) { done = true; goto Copy; } return first;", true)]
    [DataRow("int count = 0; (first, count) = (first, 1); return first;", false)]
    [DataRow("int count = 0; (first, count) = (default(CancellationToken), 1); return first;", true)]
    [DataRow("int count = 0; ((first, count), second) = ((first, 1), second); return first;", false)]
    [DataRow("(first, second) = (second, default(CancellationToken)); return first;", true)]
    [DataRow("var saved = first; for (var i = 0; i < 2; i++) { if (Flag) { second = default; if (Flag) goto Done; else goto Done; } else { return first; } } Done: return saved;", false)]
    [DataRow("var saved = first; for (var j = 0; j < 2; j++) { for (var i = 0; i < 2; i++) { if (Flag) { second = default; goto Done; } else { return first; } } Done: ; } return saved;", true)]
    [DataRow("var saved = first; for (var i = 0; i < 2; i++) { if (Flag) { second = default; if (Flag) goto Next; goto Done; } else { return first; } Next: continue; } Done: return saved;", true)]
    [DataRow("var saved = first; for (var i = 0; i < 2; i++) { if (Flag) { second = default; goto Done; } else { return first; } } Done: return first;", true)]
    [DataRow("var saved = first; if (Flag) { second = default; goto Done; } return first; Done: return first;", true)]
    [DataRow("var saved = first; if (Flag) { second = default; goto Done; } Read: return first; Done: goto Read;", true)]
    [DataRow("var saved = first; if (Flag) { second = default; goto Done; } return first; Done: goto Finish; Finish: return saved;", false)]
    [DataRow("var saved = first; try { if (Flag) { second = default; goto Done; } return saved; } finally { saved = first; } Done: return saved;", true)]
    [DataRow("var saved = first; for (var i = 0; i < 2; i++) { if (Flag) { second = default; goto Next; } return first; Next: ; } return saved;", true)]
    [DataRow("var saved = first; try { try { if (Flag) { second = default; goto Done; } } finally { if (Flag) throw new System.Exception(); } } catch { return first; } return saved; Done: return saved;", true)]
    [DataRow("var saved = first; try { try { if (Flag) { second = default; goto Done; } } finally { if (Flag) throw new System.Exception(); } } catch { return saved; } return first; Done: return saved;", false)]
    [DataRow("var saved = first; try { if (Flag) { second = default; goto Done; } } catch { } return first; Done: return saved;", false)]
    [DataRow("var saved = first; try { try { if (Flag) { second = default; goto Done; } return first; Done: return saved; } finally { } } catch { } return saved;", false)]
    public async Task Checks_Helper_Mutation_Reachability(string body, bool reports)
    {
        await new ModuleHelperRefCancellationAnalyzerTests().Checks_Helper_Ref_Argument_Storage(
            "ref token, ref token", "", body, reports);
    }

    [TestMethod]
    [DataRow("CancellationToken local = default; second = ref first; goto Rebind; Read: return second; Rebind: second = ref local; goto Read;", true)]
    [DataRow("CancellationToken local = default; second = ref first; goto Rebind; Read: return first; Rebind: second = ref local; goto Read;", false)]
    public async Task Checks_Helper_Rebinding_Backward_Jump(string body, bool reports)
    {
        await new ModuleHelperRefCancellationAnalyzerTests().Checks_Helper_Ref_Argument_Storage(
            "ref token, ref other", "CancellationToken other = default;", body, reports);
    }
}
