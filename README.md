## Q101.BbCodeNetCore
[![NuGet Package](https://img.shields.io/nuget/v/Q101.Q101JsonMediaTypeFormatter.svg?style=for-the-badge&logo=appveyor)](https://www.nuget.org/packages/Q101.Q101JsonMediaTypeFormatter)
[![NuGet Package](https://img.shields.io/nuget/dt/Q101.Q101JsonMediaTypeFormatter.svg?style=for-the-badge&logo=appveyor)](https://www.nuget.org/packages/Q101.Q101JsonMediaTypeFormatter)


Q101.BbCodeNetCore is a stable and performant BBCode-Parser for .NET/C#. Transform any BBCode into HTML. All tags are fully customizable.

The library Codekicker.BBCode has been remaked [https://archive.codeplex.com/?p=bbcode](https://archive.codeplex.com/?p=bbcode "") for the platform .Net Core version not lower 2.2
Old library is licensed under the Creative Commons Attribution 3.0 Licence: [http://creativecommons.org/licenses/by/3.0/](http://creativecommons.org/licenses/by/3.0/ "")

Made a little code refactoring

[Link to the source library developers site]([url:http://codekicker.de] "")

 
To install this assembly (class library) on the package manager console tab, run the following command:
 
```bash

   Install-Package Q101.BbCodeNetCore

```

Quickstart
Just call BBCode.ToHtml("My [b]BBCode[/b] [i]here[/i]")

```cs

		var html = BbCode.ToHtml("My [b]BBCode[/b] [i]here[/i]");

```

If you use Asp.Net, create extension class for HtmlHelper:

```cs

	public static class HtmlHelperBbCodeExtensions
	{
		public static IHtmlContent RenderBbCode(this IHtmlHelper helper, string bbcodeValue)
		{
			var html = BbCode.ToHtml(bbcodeValue);
			
			return helper.Raw(html);
		}
		
	}

```


