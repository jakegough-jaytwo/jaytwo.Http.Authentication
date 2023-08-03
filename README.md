# jaytwo.Http.Authentication

<p align="center">
  <a href="https://jenkins.jaytwo.com/job/github-jakegough-jaytwo/job/jaytwo.Http.Authentication/job/master/" alt="Build Status (master)">
    <img src="https://jenkins.jaytwo.com/buildStatus/icon?job=github-jakegough-jaytwo%2Fjaytwo.Http.Authentication%2Fmaster&subject=build%20(master)" /></a>
  <a href="https://jenkins.jaytwo.com/job/github-jakegough-jaytwo/job/jaytwo.Http.Authentication/job/develop/" alt="Build Status (develop)">
    <img src="https://jenkins.jaytwo.com/buildStatus/icon?job=github-jakegough-jaytwo%2Fjaytwo.Http.Authentication%2Fdevelop&subject=build%20(develop)" /></a>
</p>

<p align="center">
  <a href="https://www.nuget.org/packages/jaytwo.Http.Authentication/" alt="NuGet Package jaytwo.Http.Authentication">
    <img src="https://img.shields.io/nuget/v/jaytwo.Http.Authentication.svg?logo=nuget&label=jaytwo.Http.Authentication" /></a>
  <a href="https://www.nuget.org/packages/jaytwo.Http.Authentication/" alt="NuGet Package jaytwo.Http.Authentication (beta)">
    <img src="https://img.shields.io/nuget/vpre/jaytwo.Http.Authentication.svg?logo=nuget&label=jaytwo.Http.Authentication" /></a>
</p>

## Installation

Add the NuGet package

```
PM> Install-Package jaytwo.Http.Authentication
```

## Usage

This builds on the `IHttpClient` abstraction from the `jaytwo.Http` package.  This is meant for use with the `jaytwo.FluentHttp` package.

It provides a pattern for authentication and includes a few simple HTTP auth providers: 
* Basic Auth
* Bearer Auth (aka Token Auth)

Additional auth implementions built on this package:
* [jaytwo.Http.Authentication.Digest](https://github.com/jakegough-jaytwo/jaytwo.Http.Authentication.Digest)
* [jaytwo.Http.Authentication.OpenIdConnect](https://github.com/jakegough-jaytwo/jaytwo.Http.Authentication.OpenIdConnect)


```csharp
// basic auth
var httpClient = new HttpClient().Wrap().WithBasicAuthentication("user", "pass");
```

```csharp
// bearer auth
var httpClient = new HttpClient().Wrap().WithBearerAuthentication("mytoken");
```

---

Made with &hearts; by Jake
