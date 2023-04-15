namespace Dentaku
{
    using System.Windows;
    using Dentaku.ViewModel;

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region イベントハンドラ

        /// <summary>
        /// ウィンドウロード時のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var width = this.MainGrid.ActualWidth;
            var height = this.MainGrid.RowDefinitions[0].ActualHeight;

            var viewModel = this.DataContext as MainViewModel;
            viewModel.SetFormulaControlSize((width, height));
        }

        #endregion
    }
}
