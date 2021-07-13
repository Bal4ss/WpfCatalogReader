namespace WpfCatalogReader
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MainWindowViewModel();
        }

        public MainWindowViewModel ViewModel
        {
            get => (MainWindowViewModel) DataContext;
            set => DataContext = value;
        }
    }
}