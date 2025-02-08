---
_layout: landing
---

# TsGen

Quickly create Typescript types from .NET models.

## Quickstart

1. Add TsGen to your project from Nuget.
2. Annotate your models with the \[TsGen\] Attribute.
3. Add a class that extends TsGenSettings to your project and override the appropriate properties (you will at least need the OutputDirectory property).
4. Install the TsGenCli .NET CLI tool.
5. Run "tsgen" in the root of your project.
