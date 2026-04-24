# ANTLR Grammar Regeneration

Instructions for setting up the toolchain and regenerating the ANTLR parser/lexer C# files from the `.g4` grammar sources.

## 1. Prerequisites Check

Verify that Java and the ANTLR jar are available:

```bash
java -version
ls tools/antlr-*-complete.jar
```

If either command fails, follow the setup steps below.

## 2. Java Install

Install any JDK 11 or later. `java` must be on your PATH.

- **Windows:** `winget install Microsoft.OpenJDK.21`
- **macOS:** `brew install openjdk`
- **Linux:** use your distro package manager, e.g. `sudo apt install default-jdk`

After installing, open a new terminal and confirm `java -version` works.

## 3. ANTLR Jar

The jar version must match the `Antlr4.Runtime.Standard` NuGet version in `XcssSelectors/XcssSelectors.csproj` (currently `4.9.3`, for example).

Download URL pattern:
```
https://www.antlr.org/download/antlr-{VERSION}-complete.jar
```

Save the jar to `tools/antlr-{VERSION}-complete.jar` in the repo root. This path is git-ignored; re-download if the file is missing.

Example for version 4.9.3:
```bash
curl -o tools/antlr-4.9.3-complete.jar \
  https://www.antlr.org/download/antlr-4.9.3-complete.jar
```

## 4. VSCode Config

`.vscode/settings.json` already has `customClassPath` pointing to the jar at `tools/antlr-{VERSION}-complete.jar`. If you change the ANTLR version, update the version number in that path to match.

## 5. Regeneration Command

Run from `XcssSelectors/Antlr/Xcss/` (replace `{VERSION}` and `{REPO_ROOT}` with actual values):

```bash
java -jar "{REPO_ROOT}/tools/antlr-{VERSION}-complete.jar" \
  -Dlanguage=CSharp -listener -no-visitor \
  AntlrXcssLexer.g4 AntlrXcssParser.g4
```

`java` must be on PATH. If it is not, use the full path to `java.exe` instead (e.g. `"C:\Program Files\Microsoft\jdk-21.0.x.x-hotspot\bin\java.exe"`).

## 6. After Regeneration

Run the build and tests to confirm the generated files are correct:

```bash
dotnet build XcssSelectors.sln
dotnet test XcssSelectors.sln
```

## Important

Never hand-edit the generated `.cs` files in `XcssSelectors/Antlr/Xcss/`. They are checked in for convenience but are fully derived from the `.g4` grammar files. Always regenerate from `.g4` after any grammar change.
