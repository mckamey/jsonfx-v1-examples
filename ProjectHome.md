# JsonFx.NET Examples #

Example projects built using JsonFx.NET, the JSON-savvy Ajax Framework for ASP.NET.

For environment setup help, see: http://help.jsonfx.net/instructions

For ASP.NET MVC, Web Application and Web Site Visual Studio Project Templates, see: http://jsonfx.googlecode.com


---


## "[UIToolkit](http://code.google.com/p/jsonfx-examples/source/browse/#svn/trunk/UIToolkit)" ##

  * JsonFx.NET (JBST, JSON-RPC)
  * C# / ASP.NET / Visual Studio 2005
  * jQuery 1.3.2

UIToolkit shows how to build reusable control libraries which allow sharing of JBST client-side templates, scripts, and stylesheets among many projects.

The UIToolkit is an example implementation of a full-featured TreeView control, flexible modal dialog controls, loading indicator and buttons with various states.

The UI controls and affiliated parts are compiled into a redistributable Assembly which is also available for download.


---


## "[OpenID Dialog](http://code.google.com/p/jsonfx-examples/source/browse/#svn/trunk/OpenIDDialog)" ##
  * JsonFx.NET (JBST, JSON-RPC)
  * C# / ASP.NET / Visual Studio 2008
  * DotNetOpenAuth
  * jQuery 1.3.2

The third example is an OpenID library which can be easily added to another to provide a clean UI for authenticating via various OpenID providers.

This project demonstrates how JsonFx easily allows UI components to be created as reusable libraries. Scripts, StyleSheets, and JBST UI controls may all be embedded into Assemblies and easily shared among multiple projects.

(still in development)


---


## "[CalendarApp](http://code.google.com/p/jsonfx-examples/source/browse/#svn/trunk/CalendarApp)" ##
  * JsonFx.NET (JBST, JSON-RPC)
  * C# / ASP.NET MVC / Visual Studio 2008
  * LINQ-to-SQL / C# 3.0 Anonymous Objects
  * Date.js
  * jQuery 1.3.2

The second example application is a multi-view, shared calendar app. Users can add events and view a shared calendar in the spirit of Google Calendar or Apple Mobile Me.

CalendarApp shows integration of JsonFx within an ASP.NET MVC application. Each year, month or day view of the calendar has a unique, user-friendly URL.

(still in development)


---


## "[MusicApp](http://code.google.com/p/jsonfx-examples/source/browse/#svn/trunk/MusicApp)" ##
  * JsonFx.NET (JBST, JSON-RPC)
  * C# / ASP.NET WebForms / Visual Studio 2008
  * LINQ-to-SQL / C# 3.0 Anonymous Objects
  * jQuery 1.3.2

The first example project in this series is an application that allows inline editing of music bands and their members. Users can edit artists and the members directly inline, dynamically switch between views, and navigate between genres and artists.

As an extreme example of Ajax, the "one page" application, demonstrates the power of JsonFx.NET client-side templating (JBST) and Ajax services (JSON-RPC) in combination with jQuery and LINQ.

The UI is wired up using the freedom of pure JavaScript and standards compliant markup/CSS.  Data is requested via Ajax and client-side templates are dynamically bound in the browser which allows grid column sorting and change of view-type without re-requesting the bound data.

This project uses LINQ-to-SQL as a simple DAL. For client-server communication, the JSON-RPC service bi-directionally serializes LINQ-to-SQL entities and unidirectionally serializes C# 3.0 Anonymous Objects as DTOs.