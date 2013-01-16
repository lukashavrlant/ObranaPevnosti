using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ObranaPevnosti
{
    public delegate void CallBackDelegate(SeznamVstupnichInformaci SVI);

    public partial class VstupniInformace : Form
    {
        public SeznamVstupnichInformaci SVI
        { get; private set; }

        public VstupniInformace()
        {
            InitializeComponent();

            utokComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            obranaComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            utokComboBox.SelectedIndex = 0;
            obranaComboBox.SelectedIndex = 0;
        }

        private void VstupniInformaceButton_Click(object sender, EventArgs e)
        {
            SeznamVstupnichInformaci MySVI = new SeznamVstupnichInformaci();

            MySVI.JmenoObrance = JmenoObrance.Text;
            MySVI.JmenoUtocnika = JmenoUtocnika.Text;

            MySVI.JeObrancePocitacovyHrac = obranaComboBox.SelectedIndex == 1;
            MySVI.JeUtocnikPocitacovyHrac = utokComboBox.SelectedIndex == 1;

            DialogResult = DialogResult.OK;
            Close();

            SVI = MySVI;
        }
    }
}
