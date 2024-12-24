using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            typeof(ICommand),
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

        public ICommand OnClearClicked
        {
            get => (ICommand)GetValue(OnClearClickedProperty);
            set => SetValue(OnClearClickedProperty, value);
        }

        private Button ClearButton;

        public ClearableTextBox()
        {
            InitializeComponent();
            InputTextBox.GotKeyboardFocus += TextInput_GotKeyboardFocus;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            // TODO find the clear button
            ClearButton = FindClearButton(this);
            if (ClearButton != null)
            {
                // You can add event handlers or manipulate the button here
                ClearButton.Click += ClearButton_OnClearClicked;
            }
            //ClearButton.Click += ClearButton_OnClearClicked;
        }
        private Button FindClearButton(DependencyObject parent)
        {
            // Traverse the visual tree to find the ClearButton
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is Button button && button.Name == "ClearButton")
                {
                    return button;
                }
                var result = FindClearButton(child);
                if (result != null) return result;
            }
            return null;
        }

        private void TextInput_GotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            // Invoke the event handler set from XAML if it exists
            GotKeyboardFocus?.Invoke(sender, e);
            //ClearButton.Visibility = Visibility.Collapsed;
        }

        private void TextInput_LostKeyboardFocus(object sender, RoutedEventArgs e)
        {
            // ClearButton.Visibility = Visibility.Collapsed;
        }

        private void ClearButton_OnClearClicked(object sender, RoutedEventArgs e)
        {
            OnClearClicked.Execute(this);
        }
    }
}
