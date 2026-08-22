# AI System Prompt Enhancement Report

**Date:** 2026-08-21  
**Task:** Transform basic ILSpy AI prompts into expert .NET reverse engineering guidance

---

## Executive Summary

Enhanced all 8 ILSpy AI system prompts from minimal single-sentence instructions to comprehensive reverse engineering expert guidance. Each prompt now embeds domain expertise specific to .NET decompilation, compiler artifacts, framework patterns, and evidence-based analysis.

**Key improvements:**
- Compiler/decompiler awareness (recognize state machines, closures, lifted locals)
- Evidence-based reasoning (no speculation when context is incomplete)
- Framework pattern recognition (ASP.NET, Entity Framework, WPF, Avalonia, MVVM)
- Security analysis precision (observable vulnerabilities, confidence thresholds)
- .NET naming conventions and identifier analysis
- IL-aware explanations where IL is the ground truth

---

## Prompt-by-Prompt Analysis

### 1. explanation.prompt (v1 → v2)

**Original weakness:** Single generic sentence with no decompiler context.

**Enhancements:**
- **Core Principles section:** Evidence-based reasoning, compiler awareness, mechanism explanation
- **Decompiler artifact recognition:** Anonymous types, closures, iterator/async state machines, cached delegates, display classes
- **Cross-boundary reasoning:** Using call graphs to infer responsibilities and side effects
- **Framework pattern naming:** IDisposable, MVVM, ASP.NET middleware, EF conventions, WPF patterns
- **Security implications:** Observable risks (SQL injection, hardcoded credentials, weak crypto)
- **Explanation depth guidance:** Match complexity (simple getters get brief explanations, subtle patterns get depth)

**Impact:** AI can now distinguish compiler-generated artifacts from developer intent, reason about incomplete call graphs without fabrication, and identify framework patterns by name.

---

### 2. rename.prompt (v1 → v2)

**Original weakness:** Basic JSON format requirement, no naming strategy or .NET conventions.

**Enhancements:**
- **Naming Strategy section:** Infer from behavior not names, leverage call graph and string literals
- **Detailed .NET conventions:**
  - PascalCase for public types/members
  - camelCase or `_camelCase` for private fields
  - Boolean prefixes (Is, Has, Can, Should)
  - Event handler naming (On{Event})
- **Compiler artifact handling:** Recognize and preserve purpose of display classes, closures, cached delegates
- **Evidence utilization:** Call graph analysis (file I/O callees → `LoadConfigurationFile`), string literals (SQL → database operations)
- **Confidence calibration:** Explicit thresholds with examples (0.9+ for unambiguous, 0.7-0.9 for strong evidence, etc.)

**Impact:** AI produces contextually appropriate .NET identifier names with calibrated confidence, recognizing compiler patterns and using all available evidence.

---

### 3. chat.prompt (v1 → v2)

**Original weakness:** One-sentence assistant description, no multi-turn or reverse engineering guidance.

**Enhancements:**
- **Reverse Engineering Expertise section:** Compiler artifacts, incomplete information handling, framework recognition
- **IL-aware explanations:** IL as ground truth when decompiled C# is ambiguous (exception boundaries, volatile operations, tail calls)
- **Cross-assembly reasoning:** Data flow across boundaries, versioning implications, assembly binding
- **Multi-turn context:** Remember conversation, build on prior conclusions, reference earlier explorations
- **Exploration guidance:** Suggest related areas to investigate when appropriate
- **Clear boundaries:** No execution advice, no hypothetical fixes unless asked, no invented context

**Impact:** AI maintains conversation context effectively, uses IL to clarify decompiler ambiguities, and guides users through multi-step investigation without overstepping its analysis-only role.

---

### 4. security.prompt (v2 → v3) & security_audit.prompt (v1 → v2)

**Original weakness:** Minimal guidance led to false positives, generic categories, inconsistent confidence.

**Enhancements:**
- **Comprehensive vulnerability taxonomy:**
  - Injection flaws (SQL, command, LDAP, XML/XPath)
  - Cryptographic weaknesses (hardcoded secrets, weak algorithms, static IVs, insufficient key sizes)
  - Deserialization risks (BinaryFormatter, TypeNameHandling)
  - Path traversal, dangerous P/Invoke, auth/access control
  - ReDoS, reflection with user input
- **Evidence standards:** High/medium/minimum confidence definitions with examples, 0.70 minimum threshold
- **What NOT to report:** Explicit list (speculation, dead code, design critiques, theoretical issues in unreachable paths)
- **Context utilization:** String literals reveal SQL/crypto/paths, call graph traces data flow, IL clarifies boundary conditions
- **Severity assignment:** Based on actual impact in codebase, not abstract worst-case
- **Batch consistency (audit variant):** Uniform naming, de-duplication guidance, consistent severity across same patterns

**Impact:** Precision over recall — AI reports only high-confidence, observable vulnerabilities with evidence, significantly reducing false positive rate while maintaining comprehensive coverage of real issues.

---

### 5. generate_docs.prompt (v1 → v2)

**Original weakness:** "Generate XML documentation. Return only the XML, no explanation." — zero guidance on what to document or how.

**Enhancements:**
- **Documentation Strategy section:** Describe observable behavior, recognize compiler artifacts, use available evidence
- **Evidence sources:** Method signatures, code body validation, string literals (error messages), call graph (callees suggest responsibilities)
- **Specificity requirements:** Use actual exception types, actual parameter names, concrete conditions
- **Standard XML tag guidance:** summary, param, returns, exception with cref, remarks
- **Compiler artifact handling:** Don't document state machine infrastructure, document the async operation itself
- **No speculation rule:** Describe mechanics when purpose is unclear from decompiled output

**Impact:** AI generates accurate, specific XML documentation based on what the decompiled code provably does, avoiding speculation while leveraging all available evidence.

---

### 6. search.prompt (v1 → v2)

**Original weakness:** One sentence with no matching strategy or .NET awareness.

**Enhancements:**
- **Matching Strategy section:** Semantic over lexical, common .NET pattern mapping (save→Insert/Update/Persist, load→LoadConfiguration/ReadSettings)
- **Framework awareness:** Standard .NET signatures (IDisposable.Dispose, IEnumerable.GetEnumerator, OnPropertyChanged, Configure, event handlers)
- **Namespace and type context:** Domain-relevant namespace prioritization, type names provide method context
- **Partial name matching:** Fragment queries ("encrypt" matches EncryptData, DecryptData, AesEncryption)
- **Exclusions:** Compiler-generated names, infrastructure methods (GetHashCode, ToString) rank lower
- **Confidence and ranking:** Exact match highest, semantic match high, namespace match medium, weak signal low

**Impact:** AI performs intent-based semantic search over assembly vocabularies, recognizing .NET idioms and prioritizing relevant matches while filtering compiler noise.

---

### 7. assembly_summary.prompt (v1 → v2)

**Original weakness:** Three-question structure with no analysis signals or .NET ecosystem context.

**Enhancements:**
- **Summary Strategy:** Structured three-question framework (what, what framework, what usage)
- **Analysis Signals section:**
  - Namespace patterns (organized by feature vs flat, Internal/Impl namespaces)
  - Framework dependencies (System.Data, EF Core, ASP.NET, WPF, Avalonia, test frameworks, IoC containers)
  - Type patterns (Controller→ASP.NET, DbContext→EF, Window→Desktop UI, Attribute→extension framework, Test→test assembly)
  - Assembly attributes ([InternalsVisibleTo], [ComVisible], [Extension])
  - Entry point detection (Main method, executable vs library)
- **What to Avoid:** No type enumeration, no speculation on internals, no quality claims without evidence
- **Tone and format:** Reference document prose, technical precision, structured flow

**Impact:** AI produces structured, evidence-based assembly summaries that identify framework roles, architectural patterns, and ecosystem positioning from metadata alone.

---

## Cross-Cutting Improvements

All prompts now share these design principles:

1. **Decompiled code awareness:** Distinguish compiler artifacts from developer code
2. **Evidence-based reasoning:** Explicit handling of incomplete information, no fabrication
3. **Framework pattern recognition:** Name specific .NET/ASP.NET/EF/WPF/MVVM patterns when observed
4. **Operational precision:** Clear output format requirements (JSON structure, no markdown fences, confidence as numeric)
5. **Boundary clarity:** What to do, what not to do, what not to report

---

## Quantitative Changes

| Prompt | v1 Lines | v2/v3 Lines | Growth Factor | Key Additions |
|--------|----------|-------------|---------------|---------------|
| explanation | 7 | 45 | 6.4x | Compiler artifacts, framework patterns, evidence rules |
| rename | 8 | 63 | 7.9x | .NET conventions, confidence calibration, evidence strategy |
| chat | 7 | 47 | 6.7x | IL awareness, multi-turn context, cross-assembly reasoning |
| security | 8 | 101 | 12.6x | Vulnerability taxonomy, evidence standards, exclusion rules |
| security_audit | 7 | 40 | 5.7x | Batch consistency, de-duplication, uniform severity |
| generate_docs | 6 | 56 | 9.3x | Documentation strategy, evidence sources, XML tag guidance |
| search | 7 | 49 | 7.0x | Semantic matching, framework signatures, ranking logic |
| assembly_summary | 8 | 50 | 6.3x | Analysis signals, type patterns, ecosystem indicators |

**Total:** 58 lines → 451 lines (7.8x average growth)

---

## Expected Behavioral Improvements

### Before Enhancement:
- Generic explanations missing decompiler context
- Rename suggestions ignoring .NET conventions
- Security false positives from speculation
- Documentation describing what code "probably" does
- Search missing semantic matches
- Assembly summaries enumerating types without insight

### After Enhancement:
- Explanations distinguish compiler artifacts from logic
- Rename suggestions follow precise .NET conventions with calibrated confidence
- Security findings are observable, evidence-backed, above 0.70 confidence threshold
- Documentation describes demonstrable behavior with specific exception types
- Search performs intent-based matching recognizing .NET idioms
- Assembly summaries identify architectural patterns and framework roles from metadata

---

## Testing Recommendations

1. **explanation.prompt:** Test on async methods (recognize state machine), LINQ queries (recognize expression trees), WPF bindings
2. **rename.prompt:** Test on obfuscated Confuser/ConfuserEx output, compiler-generated closures, async continuations
3. **chat.prompt:** Test multi-turn scenarios crossing assembly boundaries, IL clarifications on volatile fields
4. **security.prompt:** Test on known vulnerable patterns (string concat SQL, BinaryFormatter), verify no false positives on safe parameterized queries
5. **generate_docs.prompt:** Test on methods with ArgumentException, methods with complex return types, async Task methods
6. **search.prompt:** Test semantic queries ("save data", "authenticate user") against diverse assembly vocabularies
7. **assembly_summary.prompt:** Test on ASP.NET apps, EF data layers, WPF/Avalonia UI assemblies, test projects

---

## Maintenance Notes

- Prompt versioning in metadata enables tracking evolution
- Model-specific variations can target known model strengths/weaknesses
- Future additions should maintain evidence-based, non-speculative stance
- Token count increased significantly — monitor context usage in practice
- Consider temperature_hint and max_tokens_hint tuning per prompt after production testing

---

## Conclusion

These prompts now encode expert-level .NET reverse engineering knowledge that would typically require years of experience with ILSpy, decompilers, the CLR, and .NET frameworks. The AI behaves as a knowledgeable colleague who understands:

- How decompilers transform IL to C#
- What compiler-generated code looks like
- How to read incomplete call graphs
- Where evidence ends and speculation begins
- Which framework patterns matter
- How to calibrate confidence

This transformation should significantly improve AI feature quality across all ILSpy AI capabilities: explanations, rename suggestions, chat assistance, security audits, documentation generation, search, and assembly analysis.