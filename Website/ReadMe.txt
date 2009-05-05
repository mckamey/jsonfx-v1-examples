JsonFx Examples: MusicApp
- JsonFx.NET (JBST, JSON-RPC)
- C# / ASP.NET
- jQuery
- LINQ-to-SQL / C# 3.0 Anonymous Objects
- Visual Studio 2008

The first example project in this series is an application
that allows inline editing of music bands and their members.
Users can edit artists and the members directly inline,
dynamically switch between views, and navigate between genres
and artists.

This demonstrates the power of JsonFx.NET client-side
templating (JBST) and Ajax services (JSON-RPC) in combination
with jQuery and LINQ.

The UI is wired up using the freedom of pure JavaScript and
standards compliant markup/CSS.  Data is requested via Ajax
and client-side templates are dynamically bound in the browser
which allows grid column sorting and change of view-type
without re-requesting the bound data.

This project uses LINQ-to-SQL as a simple DAL. For client-server
communication, the JSON-RPC service bi-directionally serializes
LINQ-to-SQL entities and unidirectionally serializes
C# 3.0 Anonymous Objects as DTOs.