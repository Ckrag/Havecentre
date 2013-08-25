using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Havecenter_Adressebog
{
    public partial class Settings : Form
    {
        Havecentre parent_form;
        string xml_source;
        public Settings(Havecentre parent_form_reference)
        {
            InitializeComponent();
            parent_form = parent_form_reference;
            PopulateComboBox();
        }

        private void PopulateComboBox()
        {
            source_box.Items.Add(parent_form.URL);
            source_box.Items.Add(parent_form.CACHE);

            int i = 0;
            foreach (string item in source_box.Items)
            {
                if (item == Properties.Settings.Default.xml_source)
                {
                    source_box.SelectedIndex = i;
                }
                i++;
            }
            
        }

        private void source_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)source_box.SelectedItem == parent_form.CACHE && !(File.Exists(parent_form.CACHE)))
            {
                MessageBox.Show("Cache kan ikke anvendes før listen har været hentet fra nettet mindst en gang");
                source_box.SelectedIndex = 0;
            }
            else
            {
                Properties.Settings.Default.xml_source = (string)source_box.SelectedItem;
                xml_source = (string)source_box.SelectedItem;
            }
        }

        private void save_settings_btn_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            parent_form.SetLabelSource(xml_source);
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
