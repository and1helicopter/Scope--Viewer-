using System;
using System.Windows.Forms;

namespace ScopeViewer
{
    /// <summary>
    /// Interaction logic for SaveToCOMTRADE.xaml
    /// </summary>
    public partial class SaveToComtrade 
    {
        private readonly Oscil _currentOscil;
        private bool _version;

        public SaveToComtrade(Oscil oscil, bool ver)
        {
            _currentOscil = oscil;
            InitializeComponent();

            _version = ver;

            TextBoxStationName.Text = oscil.OscilStationName.Normalize();
            TextBoxRecordingDevice.Text = oscil.OscilRecordingDevice.Normalize();
            TextBoxNominalFrequency.Text = oscil.OscilNominalFrequency.Normalize();

            TextBoxTimeCode.Text = oscil.OscilTimeCode.Normalize();
            TextBoxLocalCode.Text = oscil.OscilLocalCode.Normalize();

            TextBoxTmqCode.Text = oscil.OscilTmqCode.Normalize();
            TextBoxLeapsec.Text = oscil.OscilLeapsec.Normalize();

            if (!ver)
            {
                TextBoxTimeCode.IsEnabled = false;
                TextBoxLocalCode.IsEnabled = false;
                TextBoxTmqCode.IsEnabled = false;
                TextBoxLeapsec.IsEnabled = false;
            }
            else
            {
                TextBoxTimeCode.IsEnabled = true;
                TextBoxLocalCode.IsEnabled = false;
                TextBoxTmqCode.IsEnabled = true;
                TextBoxLeapsec.IsEnabled = true;
            }

        }


        private void Button_Click_OK(object sender, System.Windows.RoutedEventArgs e)
        {
            //Проверка что все заполнено

            if (TextBoxStationName.Text != String.Empty)
                _currentOscil.OscilStationName = TextBoxStationName.Text;
            else
            {
                MessageBox.Show(@"Не заполненное поле!", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (TextBoxRecordingDevice.Text != String.Empty)
                _currentOscil.OscilRecordingDevice = TextBoxRecordingDevice.Text;
            else
            {
                MessageBox.Show(@"Не заполненное поле!", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (TextBoxNominalFrequency.Text != String.Empty)
                _currentOscil.OscilNominalFrequency = TextBoxNominalFrequency.Text;
            else
            {
                MessageBox.Show(@"Не заполненное поле!", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (_version)
            {
                if (TextBoxTimeCode.Text != String.Empty)
                    _currentOscil.OscilTimeCode = TextBoxTimeCode.Text;
                else
                {
                    MessageBox.Show(@"Не заполненное поле!", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (TextBoxLocalCode.Text != String.Empty)
                    _currentOscil.OscilLocalCode = TextBoxLocalCode.Text;
                else
                {
                    MessageBox.Show(@"Не заполненное поле!", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (TextBoxTmqCode.Text != String.Empty)
                    _currentOscil.OscilTmqCode = TextBoxTmqCode.Text;
                else
                {
                    MessageBox.Show(@"Не заполненное поле!", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (TextBoxLeapsec.Text != String.Empty)
                    _currentOscil.OscilLeapsec = TextBoxLeapsec.Text;
                else
                {
                    MessageBox.Show(@"Не заполненное поле!", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            DialogResult = true;
        }

        private void Button_Click_Cancel(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
