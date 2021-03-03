# GihanSoft Navigation

[![publish to nuget](https://github.com/GihanSoft/GihanSoft.Navigation/actions/workflows/publish.yml/badge.svg)](https://github.com/GihanSoft/GihanSoft.Navigation/actions/workflows/publish.yml)

## usage:

```xml
<nav:PageHost 
    xmlns:nav="http://gihansoft.ir/netfx/xaml/navigation"
    Name="PageHost" />
```

**Important!!** do this where used `PageHost`:
```csharp
    public MainWindow()
    {
        this.InitializeComponent();
        //...
        this.PageHost.PageNavigator = new GihanSoft.Navigation.PageNavigator(App.Current.ServiceProvider);
        //...
    }
```

use left & right toolbars like this:
```xml
<ContentControl Grid.Column="1" Content="{Binding PageNavigator.Current.LeftToolBar, ElementName=PageHost}" />
```
navigatoin:
```csharp
//...
this.PageHost.PageNavigator.GoTo<PgMain>();
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
</nav:Page>
```