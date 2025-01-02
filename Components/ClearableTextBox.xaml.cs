using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Circle_2.Components
{
    /// <summary>
    /// Interaction logic for ClearableTextBox.xaml
    /// </summary>
    [TemplatePart(Name = "ClearButton", Type = typeof(Button))]
    public partial class ClearableTextBox : UserControl
    {

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),//name of the property
            typeof(string), // type of property
            typeof(ClearableTextBox), // owner name of the property
            new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty GotKeyboardFocusProperty = DependencyProperty.Register(
            nameof(GotKeyboardFocus),//name of the property
            typeof(RoutedEventHandler), // type of property
            typeof(ClearableTextBox), // owner name of the property
            new PropertyMetadata(null));

        public static readonly DependencyProperty OnClearClickedProperty = DependencyProperty.Register(
            nameof(OnClearClicked),
            typeof(RoutedEventHandler),
            typeof(ClearableTextBox),
            new PropertyMetadata(null));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public new RoutedEventHandler GotKeyboardFocus
        {
            get => (RoutedEventHandler)GetValue(GotKeyboardFocusProperty);
            set => SetValue(GotKeyboardFocusProperty, value);
        }

        public RoutedEventHandler OnClearClicked
        {
            get => (RoutedEventHandler)GetValue(OnClearClickedProperty);
            set => SetValue(OnClearClickedProperty, value);
        }

        private Button? ClearButton = null;
        private SolidColorBrush ActiveBrush = new BrushConverter().ConvertFrom("#D1D1D1") as SolidColorBrush;
        private SolidColorBrush InactiveBrush = new SolidColorBrush(Colors.White);

        public ClearableTextBox()
        {
            InitializeComponent();
            Loaded += ComponentLoaded;
            InputTextBox.GotKeyboardFocus += TextInput_GotKeyboardFocus;
            InputTextBox.LostKeyboardFocus += TextInput_LostKeyboardFocus;
        }

        private void ComponentLoaded(object sender, RoutedEventArgs e)
        {
            ClearButton = InputTextBox.Template.FindName("ClearButton", InputTextBox) as Button;
            if (ClearButton != null)
            {
                // You can add event handlers or manipulate the button here
                ClearButton.Click += ClearButton_OnClearClicked;
            }
        }


        private void TextInput_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            // ignore the event if it is coming from the button click
            if (e.NewFocus is TextBox)
            {
                GotKeyboardFocus(sender, e);
                InputTextBox.Background = ActiveBrush;
            }
        }

        private void TextInput_LostKeyboardFocus(object sender, RoutedEventArgs e)
        {
            InputTextBox.Background = InactiveBrush;
        }

        private void ClearButton_OnClearClicked(object sender, RoutedEventArgs e)
        {
            OnClearClicked?.Invoke(InputTextBox, e);
        }
    }
}
