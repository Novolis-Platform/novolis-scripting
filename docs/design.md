# Design

The scripting family has three responsibilities:

- `CSharp` runs snippets with controlled references, imports, and globals.
- `Completion` reports source diagnostics for editor surfaces.
- `Hosting` composes both without bringing UI dependencies into the library.

Avalonia, Blazor, CLI, and lab hosts choose their own editor and safety policy.
