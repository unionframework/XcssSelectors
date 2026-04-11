# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What This Is

`xcss-parser-csharp` — a .NET 6.0 library that parses XCSS selectors (CSS-like syntax with extensions) and converts them to XPath expressions. The CSS output path is not implemented.

## Build and Test Commands

```bash
# Build
dotnet build XcssSelectors.sln

# Run all tests
dotnet test XcssSelectors.sln

# Run a single test by name filter
dotnet test XcssSelectors.sln --filter "FullyQualifiedName~XPathBuilderTests"

# Verbose test output
dotnet test XcssSelectors.sln -v detailed
```

## Architecture

### Pipeline

```
XCSS.Parse(string)
  → XcssParser.Parse()
      → ANTLR4 (Lexer → TokenStream → Parser → ParseTree)
      → CollectXcssSelectorsListener (walks tree, fills models)
  → List<XcssSelectorData>
  → XPathBuilder.Build() → XPath string
```

### Key Types

**`XCSS`** (`XCSS.cs`) — Public facade. `XCSS.Parse(selector)` returns an object with `.XPath` and `.Selector` properties.

**`XcssParser`** (`Parsers/XcssParser.cs`) — Wires ANTLR runtime: creates input stream, lexer, token stream, parser, attaches listener, then triggers a walk.

**`CollectXcssSelectorsListener`** (`Parsers/CollectXcssSelectorsListener.cs`) — ANTLR `BaseListener` implementation. Uses a `Stack<XcssSelectorContext>` to handle nested selectors. Populates `List<XcssSelectorData>` during tree walk.

**`XPathBuilder`** (`Builders/XPathBuilder.cs`) — Converts models to XPath strings. Key methods:
- `Build(XcssElementData)` — single element with conditions
- `Build(XcssSelectorData)` — element chain
- `Combine(IEnumerable<string>)` — joins with XPath `|`
- `Concat(root, relative)` — axis-aware concatenation

**Models** (`Models/`) — Plain data: `XcssSelectorData` → `List<XcssElementData>` → attributes, text conditions, sub-element selectors, XPath condition strings.

### XCSS Syntax Extensions Over CSS

| XCSS Syntax | XPath Output |
|---|---|
| `div['text']` | `//div[text()[normalize-space(.)='text']]` |
| `div[~'text']` | `//div[text()[contains(normalize-space(.),'text')]]` |
| `li[>a[@disabled]]` | nested element as predicate |
| `input[translate(@type,'B','b')='button']` | raw XPath condition passthrough |
| `div, span` | `//div \| //span` |

Combinator → XPath axis mapping: ` ` (space) = `descendant::`, `>` = `child::`, `+`/`~` = `following-sibling::`.

### ANTLR Grammar

Grammars are in `XcssSelectors/Antlr/Xcss/`. The `.g4` files are the source; the `.cs` files are generated output (checked in). If grammars change, regenerate with the VSCode ANTLR4 extension (config in `.vscode/settings.json`: external mode, C#, listeners only, no visitors).

### Known Gaps

- `CssBuilder.BuildFromParts()` throws `NotImplementedException` — CSS output is not implemented.
- `XcssOptions` enum is empty — no option handling exists.
- Attribute `^=` (prefix) and `$=` (suffix) match styles are modeled but conversion logic is incomplete.
- `:not()` pseudo-class is stubbed, not implemented.
