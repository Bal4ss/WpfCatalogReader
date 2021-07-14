namespace WpfCatalogReader
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MainWindowViewModel
            {
                UiItems = ItemsWrapperPanel.Children,
                UiIds = ItemsIdsPanel.Children
            };
        }

        public MainWindowViewModel ViewModel
        {
            get => (MainWindowViewModel) DataContext;
            set => DataContext = value;
        }
    }
}