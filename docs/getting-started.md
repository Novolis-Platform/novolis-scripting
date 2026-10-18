# Getting started

```powershell
dotnet add package Novolis.Scripting.Hosting
```

```csharp
var service = new CSharpScriptService();
var diagnostics = service.Analyze("1 +");
var result = await service.ExecuteAsync("1 + 2");
```

Supply explicit assembly references, imports, and globals through `ScriptHostOptions`. Hosts decide which APIs are safe to expose.
