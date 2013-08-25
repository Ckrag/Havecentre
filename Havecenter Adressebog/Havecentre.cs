using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.Xml;
using Havecenter_Adressebog.Properties;
using System.Configuration;
using System.Net;


namespace Havecenter_Adressebog
{
    public partial class Havecentre : Form
    {   
        public List<Center> havecentre_list = new List<Center>();
        private static string file_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Havecenter Adressebog");
        private static string file_name = "Havecenter brevliste.pdf";
        private static string full_path = Path.Combine(file_path,file_name);
        public string URL = "http://mail.danskehavecentre.dk:555/vimedhave/danskehavecentre.aspx";
        public string CACHE = Path.Combine(file_path, "cache.xml");

        public void AddCenters()
        {
            havecentre_list = havecentre_list.OrderBy(o => o.Name).ToList();
            xml_centers.DataSource = havecentre_list.Select(o => o.Name).ToList();
            status_text_label.Text = "Klar til brug";
            load_img.Hide();
        }
        
        public Havecentre()
        {
            InitializeComponent();
            TryReusePDF();
            SetLabelSource(Properties.Settings.Default.xml_source);
            status_text_label.Text = "Klar til brug";
            load_img.Hide();
        }

        private void load_xml_Click(object sender, EventArgs e)
        {
            bool cache_error = false;
            bool internet_error = false;

            if (Properties.Settings.Default.xml_source == URL)
            {
                try
                {
                    StartLoadingData();
                }
                catch
                {
                    Properties.Settings.Default.xml_source = CACHE;
                    Properties.Settings.Default.Save();
                    SetLabelSource(CACHE);
                    MessageBox.Show("Der skete en internet-relateret fejl. Tjek internetforbindelsen og prøv igen\n\nHvis denne fejl fortsætter kontakt support.");
                    internet_error = true;
                }
            }


            else if (Directory.Exists(file_path))
            { 
                if (!(File.Exists(CACHE)))
                {
                    Properties.Settings.Default.xml_source = URL;
                    Properties.Settings.Default.Save();
                    SetLabelSource(URL);
                    cache_error = true;
                    if (!(cache_error && internet_error))
                    {
                        MessageBox.Show("Der kunne ikke findes en internet adgang eller en cache. Programmet kræver internetadgang første gang det køres");
                        return;
                    }
                }
                StartLoadingData();
            }
            //StartLoadingData();     
        }

        private void StartLoadingData()
        {
            status_text_label.Text = "..henter data, vent venligst..";
            load_img.Show();
            Refresh();
            
            CreateBackup();
            Loading loading_form = new Loading(this);
            loading_form.Show();
        }

        private void xml_centers_SelectedIndexChanged(object sender, EventArgs e)
        {
            xml_name.Text = havecentre_list[xml_centers.SelectedIndex].Name;
            xml_attention.Text = havecentre_list[xml_centers.SelectedIndex].Attention;
            xml_address.Text = havecentre_list[xml_centers.SelectedIndex].Address;
            xml_zip.Text = havecentre_list[xml_centers.SelectedIndex].Zip;
            xml_city.Text = havecentre_list[xml_centers.SelectedIndex].City;
            xml_phone.Text = havecentre_list[xml_centers.SelectedIndex].Phone;
            xml_email.Text = havecentre_list[xml_centers.SelectedIndex].Email;
            xml_url.Text = havecentre_list[xml_centers.SelectedIndex].Url;
        }

        private string populate_email_list()
        {
            string mail_string = null;

            foreach(Center email in havecentre_list)
            {
                if(email.Email != ""){
                    mail_string += email.Email + ", ";
                }
            }
            return mail_string;
        }
 
        private void button1_Click(object sender, EventArgs e)
        {
            if (havecentre_list.Count == 0)
            {
                MessageBox.Show("Der er ingen information at vise, tryk 'Hent Information' for at hente den");
            }
            else
            {
                xml_email_list.Text = "";
                xml_email_list.Text = populate_email_list();
            }
        }

        private void TryReusePDF()
        {
            if (File.Exists(full_path))
            {
                if (MessageBox.Show("Der er fundet et tidligere PDF dokument, vil du genbruge dette? \n\nJa - Lukker programmet og genåbner det PDF du lavede sidst. \nNej - Åbner programmet på normal vis.", "Genåben Tidligere PDF", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Process.Start(full_path);
                    Environment.Exit(0);
                }
            }
        }

        private void create_pdf_btn_Click(object sender, EventArgs e)
        {
            Document doc = new Document(iTextSharp.text.PageSize.A4, 58, 58, 108, 10);
            if (havecentre_list.Count == 0)
            {
                MessageBox.Show("Der er ingen information at vise, tryk 'Hent Information' for at hente den");
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(file_path);
                    var output = new FileStream(full_path, FileMode.Create);
                    var writer = PdfWriter.GetInstance(doc, output);

                    doc.Open();

                    int page_counter = 0;
                    foreach (Center center in havecentre_list)
                    {
                        if (center.Name != "" && center.Address != "" && center.Zip != "")
                        {

                            if (page_counter > 0)
                            {
                                doc.NewPage();
                            }
                            string name = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(center.Name.ToLower());
                            string address = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(center.Address.ToLower());
                            string city = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(center.City.ToLower());
                            doc.Add(new Phrase(name));
                            doc.Add(new Phrase(Environment.NewLine));
                            doc.Add(new Phrase(address));
                            doc.Add(new Phrase(Environment.NewLine));
                            doc.Add(new Phrase(center.Zip + " " + city));
                            page_counter++;
                        }
                    }
                    doc.Close();
                    Process.Start(full_path);
                    Application.Exit();
                }
                catch
                {
                    MessageBox.Show("Kunne ikke gemme da PDF'et allerede er åbent. \nHvis dette fortsætter, kontakt udvikler");
                }
            }           
        }

        public void ShowError(bool error, string reason)
        {
            if (error)
            {
                MessageBox.Show(reason);
            }
        }

        private void CreateBackup()
        {
            try
            {//backup
                Directory.CreateDirectory(file_path);

                XmlNode backup_node = new XmlDocument();

                XmlDocument doc = new XmlDocument();
                doc.Load(Properties.Settings.Default.xml_source);

                backup_node = doc.SelectSingleNode("Havecentre");

                File.Create(Path.Combine(file_path, "cache.xml")).Close();

                XmlDocument backup = new XmlDocument();

                XmlTextWriter writer = new XmlTextWriter(Path.Combine(file_path, "cache.xml"), Encoding.UTF8);
                writer.Formatting = Formatting.Indented;

                backup_node.WriteTo(writer);
                writer.Close();
            }
            catch
            {
            }
        }

        private void settings_btn_Click(object sender, EventArgs e)
        {
            Settings settings_form = new Settings(this);
            settings_form.Show(); 
        }

        public void SetLabelSource(string source)
        {
            source_label.Text = "XML Kilde: " + source;
        }
    }
}
