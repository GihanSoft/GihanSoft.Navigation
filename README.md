# GihanSoft Navigation

[![publish to nuget](https://github.com/GihanSoft/GihanSoft.Navigation/actions/workflows/publish.yml/badge.svg)](https://github.com/GihanSoft/GihanSoft.Navigation/actions/workflows/publish.yml)

## usage:

```xml
<Window x:Class="Lab.MainWindow"
    ...
    ...
    Name="This">
```

**Important!!** do this where used `PageHost`:
```csharp
    private static readonly DependencyPropertyKey PageNavigatorPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(PageNavigator),
            typeof(PageNavigator),
            typeof(MainWindow),
            new PropertyMetadata(default(PageNavigator)));

    /// <summary>Identifies the <see cref="PageNavigator"/> dependency property.</summary>
    public static readonly DependencyProperty PageNavigatorProperty = PageNavigatorPropertyKey.DependencyProperty;

    public MainWindow()
    {
        this.InitializeComponent();
        ...
        ...
        // put a service provider in app.xaml.cs
        this.PageNavigator = new PageNavigator(App.Current.ServiceProvider);
        ...
        ...
    }

    /// <summary>
    /// Gets Page Navigator.
    /// </summary>
    public PageNavigator? PageNavigator
    {
        get => (PageNavigator?)this.GetValue(PageNavigatorProperty);
        private set => this.SetValue(PageNavigatorPropertyKey, value);
    }
```

use left & right toolbars like this:
```xml
<ContentControl Grid.Column="1" Content="{Binding PageNavigator.CurrentPage.LeftToolBar, ElementName=This}" />
```
navigatoin:
```csharp
//...
this.PageHost.PageNavigator.GoToAsync<PgMain>();
//...
```
YourPage.xaml:
```xml
<nav:Page
    x:Class="Your.NameSpace.PgMain"
    xmlns:nav="http://gihansoft.ir/netfx/xaml/navigation">
    <!-- optional -->
    <nav:Page.LeftToolBar>
        <ToolBar>
        </ToolBar>
    </nav:Page.LeftToolBar>
    <!-- page content -->
</nav:Page>
```