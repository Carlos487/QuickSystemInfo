using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace QuickSystemInfo_Avalonia.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //CanResize = false;
            //SizeToContent = SizeToContent.WidthAndHeight;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}