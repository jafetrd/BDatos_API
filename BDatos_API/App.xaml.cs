using MahApps.Metro;
using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace BDatos_API
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // add custom accent and theme resource dictionaries to the ThemeManager
            // you should replace MahAppsMetroThemesSample with your application name
            // and correct place where your custom accent lives
            RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.SoftwareOnly;

            ThemeManager.AddAccent("CustomAccent1", new Uri("pack://application:,,,/BDatos_API;component/AcentoColor.xaml"));

            // get the current app style (theme and accent) from the application
            Tuple<AppTheme, Accent> theme = ThemeManager.DetectAppStyle(Application.Current);

            // now change app style to the custom accent and current theme
            ThemeManager.ChangeAppStyle(Application.Current,
                                        ThemeManager.GetAccent("CustomAccent1"),
                                        theme.Item1);

            base.OnStartup(e);

           
        }

    }
}
