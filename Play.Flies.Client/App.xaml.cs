using System;
using System.Linq;
using System.Windows;

namespace Play.Flies.Client
{
    /// <summary> Interaction logic for App.xaml. </summary>
    public partial class App
    {
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            foreach (var disposable in Resources.Values.OfType<IDisposable>())
            {
                disposable.Dispose();
            }
        }
    }
}