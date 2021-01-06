# NSelene - User-oriented Web UI browser tests in .NET (Selenide port from Java)

![Free](https://img.shields.io/badge/free-open--source-green.svg)
[![MIT License](http://img.shields.io/badge/license-MIT-green.svg)](https://github.com/yashaka/nselene/blob/master/LICENSE)

![GitHub stats in](https://raw.githubusercontent.com/yashaka/NSelene/traffic/traffic-NSelene/in_2021.svg)
![GitHub views](https://raw.githubusercontent.com/yashaka/NSelene/traffic/traffic-NSelene/views.svg)
![GitHub views per week](https://raw.githubusercontent.com/yashaka/NSelene/traffic/traffic-NSelene/views_per_week.svg)
![GitHub clones](https://raw.githubusercontent.com/yashaka/NSelene/traffic/traffic-NSelene/clones.svg)
![GitHub clones per week](https://raw.githubusercontent.com/yashaka/NSelene/traffic/traffic-NSelene/clones_per_week.svg)

[![Join telegram chat https://t.me/nselene](https://img.shields.io/badge/chat-telegram-blue)](https://t.me/nselene)
[![Присоединяйся к чату https://t.me/nselene_ru](https://img.shields.io/badge/%D1%87%D0%B0%D1%82-telegram-red)](https://t.me/nselene_ru)


Main features:

- **User-oriented API for Selenium Webdriver** (code like speak common English)
- **Ajax support** (Smart implicit waiting and retry mechanism)
- **PageObjects support** (all elements are lazy-evaluated objects)
- **Automatic driver management** (no need to install and setup driver for quick local execution)

[Available at Nuget](www.nuget.org/packages/NSelene)

Tests with Selene can be built either in a simple straightforward "selenide' style or with PageObjects composed from Widgets i.e. reusable element components.

For docs see tests in the [NSeleneTests](https://github.com/yashaka/NSelene/blob/master/NSeleneTests) project for now;)

## Table of Content
* [Versions](#versions)
* [Overview](#overview)
* [Contributing](#contributing)
* [Release Process](#release-process)

## Versions
  
* Upcomig version to use is just released [1.0.0-alpha03](https://www.nuget.org/packages/NSelene/1.0.0-alpha03)
  * targets netstandard2.0
    * net45 support may be added later
  * it differs from [0.0.0.7](https://www.nuget.org/packages/NSelene/0.0.0.7) in the following:
    * repacked in sdk-style format
    * removed things marked as obsolete til 0.0.0.7
    * added some small features, see CHANGELOG for more details
  * can be installed by:
    `dotnet add package NSelene --version 1.0.0-alpha03`

* Latest stable version: [0.0.0.7](https://www.nuget.org/packages/NSelene/0.0.0.7)
  * targets net45
  * it is main version used by most nselene users during during last 4 years
  * there were not so much users, but even some small companies use it for their projects
  * so it was proven to be stable for production use
  * its sources can be found at [0.x](https://github.com/yashaka/nselene/tree/0.x) branch

See [changelog](https://github.com/yashaka/NSelene/blob/master/CHANGELOG.md) for more details.

## Overview

Find an example of NSelene usage in [this template project](https://github.com/yashaka/Web.Tests.Net).

Below you can find a short overview:

NSelene has no fully automatic driver management. Given the [WebDriverManager] is installed, then you can set your driver up manually, e.g. like here: 

```csharp
    [TestFixture]
    public class BrowserTest
    {
        [SetUp]
        public void InitDriver()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            Selene.SetWebDriver(new ChromeDriver());
        }

        [TearDown]
        public void QuitDriver()
        {
            Selene.GetWebDriver().Quit();
        }
    }
```

Tests may look like this in a so-called "straightforward" style:

```csharp
    [TestFixture]
    public class TodoMvcShould : BrowserTest
    {
        [Test]
        public void CompleteTask()
        {
            Open("http://todomvc.com/examples/emberjs/");
            S("#new-todo").SetValue("a").PressEnter();
            S("#new-todo").SetValue("b").PressEnter();
            S("#new-todo").SetValue("c").PressEnter();

            SS("#todo-list>li").FindBy(Have.ExactText("b")).Find(".toggle").Click();

            SS("#todo-list>li").FilterBy(Have.CssClass("completed")).Should(Have.ExactTexts("b"));
            SS("#todo-list>li").FilterBy(Have.No.CssClass("completed")).Should(Have.ExactTexts("a", "c"));
        }
    }
```

or with PageObjects:

```csharp
using NUnit.Framework;
using Web.Tests.Model;

namespace Web.Tests
{
    public class SearchEnginesShouldSearch : BrowserTest
    {
        [Test]
        public void Ecosia()
        {
            Www.ecosia.Open();

            Www.ecosia.Search("nselene dotnet");
            Www.ecosia.Results.ShouldHaveSizeAtLeast(5)
                .ShouldHaveText(0, "Consise API to Selenium for .Net");

            Www.ecosia.Results.FollowLink(0);
            Www.github.ShouldBeOn("yashaka/NSelene");
        }
    }
}
```

where the Ecosia page object may look like this:

```csharp
namespace Web.Tests.Model.Pages
{
    public class Ecosia 
    {
        public Results Results => new Results(SS(".js-result"));

        public void Open()
        {
            Selene.Open("https://www.ecosia.org/");
        }

        public void Search(string text)
        {
            S(By.Name("q")).Type(text).PressEnter();
        }
    }

    public class Github
    {
        public void ShouldBeOn(string pageTitleText)
        {
            Selene.WaitTo(Match.TitleContaining(pageTitleText));
        }
    }
}

// ...

namespace Web.Tests.Model.Common
{
    public class Results
    {
        SeleneCollection list;

        public Results(SeleneCollection list)
        {
            this.list = list;
        }

        public Results ShouldHaveSizeAtLeast(int number)
        {
            list.Should(Have.CountAtLeast(number));
            return this;
        }

        public Results ShouldHaveText(int index, string value)
        {
            list[index].Should(Have.Text(value));
            return this;
        }

        public void FollowLink(int index)
        {
            list[index].Find("a").Click();
        }
    }
}

// ...

namespace Web.Tests.Model
{
    class Www
    {
        public static Duckduckgo duckduckgo = new Duckduckgo();
        public static Ecosia ecosia = new Ecosia();
        public static Github github = new Github();
    }
}
```

So... 
The main things are ported: 
- Selenide.$
- SelenideElement#should
- SelenideElement#shouldNot
- SelenideElement#find / SelenideElement#$
- SelenideElement#findAll / SelenideElement#$$
- Selenide.$$
- EllementsCollection#should
- EllementsCollection#shouldNot
- EllementsCollection#findBy
- EllementsCollection#filterBy
- EllementsCollection#get
- all main Conditions
- all main CollectionConditions

Though NSelene is not a "complete" port (no automatic screenshots, no automatic driver creation, etc.), even now It has some useful additions to the basic "selenide set" of features. The following a some features that are still absent in Selenide:

NSelene has a flexible way to locate elements by breakig long especially xpath selectors to smaller parts, like in:

```
SS("#list-item").FindBy(Have.CssClass("specific")).Find(".inner-element").click()
```

This allows to idetify more precisely which part was broken on failure.

`S`, `SS` also supports xpath selectors:
```
SS("(//h1|//h2)[contains(text(), 'foo')]").Should(Have.Count(10));
```

NSelene can be easily integrated into existing selenium based frameworks, because it is object-oriented by its nature. It provides a Consice API to Selenium via both "OOP" (`SeleneDriver` class) and "static utils" wrappers (`Selene` static class) over WebDriver. Because of the latter, NSelene also supports creation of "more than one driver per test". It can be rarely useful, but sometimes it "saves the life".

Feel free to share your thoughts and file an issue on github if you need something.

## Contributing

Before implementing your ideas, it is recommended first to create a corresponding issue and discuss the plan to be approved;)
Also consider first to help with issues marked with help_wanted label ;)

### Prerequisites

Target framework is .Net Standard 2.0

To build from the command line you need `dotnet` 2.0 and higher

#### Windows

* Visual Studio 2017+, VS Code or JetBrains Rider

#### MacOS or Linux

* Visual Studio for Mac, VS Code or JetBrains Rider

1. Clone project git clone https://github.com/yashaka/NSelene.git
2. cd NSelene
3. For command line use `dotnet build`, or build solution using your code editor.
4. Add a "feature request" Issue to this project.
5. Discuss its need and possible implementation. And once approved...
6. Fork the project ( https://github.com/[my-github-username]/NSelene/fork )
7. Create your feature branch (`git checkout -b my-new-feature`)
8. Commit your changes (`git commit -am 'Add some feature'`)
9. Push to the branch (`git push origin my-new-feature`)
10. Create a new Pull Request`

## Release process

### Given

- the version is bumped in the `NSelene.csproj` to the next one, comparing to the peviously released version.
- the changelog has a section for the current version (to be released) with list of all features implemented since the previous version was released
  - you can check the commits history and closed issues since previous release
- the readme is updated if needed
- tests pass
  ```
  cd NSeleneTests
  dotnet test
  cd ../NSelene
  ```
- the corresponding git tag with current version number and description (should reflect the changelog) is added 

### Then

```
dotnet build -c Release
dotnet nuget push bin/Release/NSelene<VERSION>.nupkg -k <KEY> -s https://api.nuget.org/v3/index.json
```
