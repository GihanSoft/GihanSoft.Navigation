using System.Windows;
using System.Windows.Markup;

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None,
    ResourceDictionaryLocation.SourceAssembly)]
/* where theme specific resource dictionaries are located
 * (used if a resource is not found in the page, or application resource dictionaries)
 *
 * where the generic resource dictionary is located
 * (used if a resource is not found in the page app, or any theme specific resource dictionaries)
 */

[assembly: CLSCompliant(true)]
[assembly: XmlnsDefinition(
    "http://gihansoft.ir/netfx/xaml/navigation",
    "GihanSoft.Navigation.WPF")]

[assembly: XmlnsPrefix("http://gihansoft.ir/netfx/xaml/navigation", "gnav")]
