using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Havecenter_Adressebog
{
    public partial class Loading : Form
    {
        Havecentre parent_form;
        bool xml_error = false;
        public Loading(Havecentre parent_form_reference)
        {
            parent_form = parent_form_reference;
            InitializeComponent();
            backgroundWorker_xml.RunWorkerAsync();
        }

        private void backgroundWorker_xml_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Properties.Settings.Default.xml_source);

                XmlNodeList xnList = doc.SelectNodes("Havecentre/Havecenter");

                parent_form.havecentre_list.Clear();
                int goal = xnList.Count;
                int progress = 0;

                foreach(XmlNode node in xnList)
                
                {
                    
                    if ((worker.CancellationPending == true))
                    {
                        e.Cancel = true;
                        break;
                    } 
                    else 
                    {
                        //for each center!
                        Center new_center = new Center();
                        foreach (XmlNode child in node)

                        {
                            switch (child.Name)
                            {
                                case "Name":
                                    new_center.Name = child.InnerText;
                                    break;
                                case "Attention":
                                    new_center.Attention = child.InnerText;
                                    break;
                                case "Address":
                                    new_center.Address = child.InnerText;
                                    break;
                                case "Zip":
                                    new_center.Zip = child.InnerText;
                                    break;
                                case "City":
                                    new_center.City = child.InnerText;
                                    break;
                                case "Phone":
                                    new_center.Phone = child.InnerText;
                                    break;
                                case "Email":
                                    new_center.Email = child.InnerText;
                                    break;
                                case "Url":
                                    new_center.Url = child.InnerText;
                                    break;
                                default:
                                    break;
                            }
                        }
                        parent_form.havecentre_list.Add(new_center); //add it to the list
                        progress++;
                        backgroundWorker_xml.ReportProgress(progress * 100 / goal);
                    }
                }

            }
            catch (Exception Ex)
            {
                xml_error = true;
            }
        }

        private void backgroundWorker_xml_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            xml_progressBar.Value = e.ProgressPercentage;
        }

        private void backgroundWorker_xml_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                xml_progressBar.Value = 0;
                close_loader();
            }
            else if (!(e.Error == null))
            {
                close_loader();
            }
            else
            {
                close_loader();
            }

        }

        private void close_loader()
        {
            Close();
            parent_form.AddCenters();
            parent_form.ShowError(xml_error, "Der skete en fejl under hentningen af data, prøv at skifte 'XML Kilde' under 'Indstillinger'");
        }

        private void cancel_xml_load_Click(object sender, EventArgs e)
        {
            backgroundWorker_xml.CancelAsync();
        }
    }
}
