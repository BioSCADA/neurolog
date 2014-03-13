/*M///////////////////////////////////////////////////////////////////////////////////////
//
//  IMPORTANT: READ BEFORE DOWNLOADING, COPYING, INSTALLING OR USING.
//
//  By downloading, copying, installing or using the software you agree to this license.
//  If you do not agree to this license, do not download, install,
//  copy or use the software.
//
//
//                           License Agreement
//                For Open Source BioSCADA® Library  
//
// Copyright (C) 2011-2012, Diego Schmaedech, all rights reserved. 
//
							For Open Source SCADA for Human Data
//
// Copyright (C) 2012, Laboratório de Educação Cerebral, all rights reserved.
//
// Copyright (C) 2013, CogniSense Tecnologia Ltda, all rights reserved.
// Third party copyrights are property of their respective owners.
// Third party copyrights are property of their respective owners.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
//   * Redistribution's of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//
//   * Redistribution's in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//
//   * The name of the copyright holders may not be used to endorse or promote products
//     derived from this software without specific prior written permission.
//
// This software is provided by the copyright holders and contributors "as is" and
// any express or implied warranties, including, but not limited to, the implied
// warranties of merchantability and fitness for a particular purpose are disclaimed.
// In no event shall the Intel Corporation or contributors be liable for any direct,
// indirect, incidental, special, exemplary, or consequential damages
// (including, but not limited to, procurement of substitute goods or services;
// loss of use, data, or profits; or business interruption) however caused
// and on any theory of liability, whether in contract, strict liability,
// or tort (including negligence or otherwise) arising in any way out of
// the use of this software, even if advised of the possibility of such damage.
//
//M*/
using Neurolog.Sessions;
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
using System.Net.Http;
using System.Xml;
using Neurolog.Blueteeth;
using System.Data;
using System.IO;
using System.Collections;
using Neurolog;
using MathLibrary;

namespace BioSCADA
{
    /// <summary>
    /// Interaction logic for ReportPanel.xaml
    /// </summary>
    public partial class ReportPanel : UserControl
    {

        int samplePointer = 0;
        int sampleCount = 0;
        string filename = "";
        DataSet ds = new DataSet();

        public ReportPanel()
        {
            InitializeComponent();
        }

        public void bt_up_Click(object sender, RoutedEventArgs e)
        {
            if (User.IsLogged)
            {
                Send(); 
            }
            else
            {
                AlarmMessageBus.log((Brush)this.TryFindResource("RedColor"), "é necessário fazer Login para usar este recurso!");
            }
            

        }

        private void bt_down_Click(object sender, RoutedEventArgs e)
        {
            if (User.IsLogged)
            {
                Down();
            }
            else
            { 
                AlarmMessageBus.log((Brush)this.TryFindResource("RedColor"), "é necessário fazer Login para usar este recurso!");
            }
            
           
        }

        private void update_file( )
        {
            filename = lb_filename.Text;
            if (File.Exists(filename))
            {
                ds = new DataSet();
                ds.ReadXml(filename, XmlReadMode.InferSchema);
                GetStats(ds);
                session_panel.FillDataGrig(ds); 
                AlarmMessageBus.log((Brush)this.TryFindResource("BlueColor"), "arquivo " + filename + " atualizado com sucesso");
            }
            else
            {
                if (User.IsLogged)
                {
                    GetSamplesFromServer();
                }
                else
                {
                    AlarmMessageBus.log((Brush)this.TryFindResource("RedColor"), "é necessário fazer Login para usar este recurso!");
                }
                
            }
             
        }


        private void lb_aquisitions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //update_file(); 
        }

        private void lb_aquisitions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            update_file(); 
        }

        private void GetStats(DataSet ds)
        {
            try
            {
                DataTable dt = ds.Tables[0];
                ArrayList ls = new ArrayList();
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        ls.Add(float.Parse(dr["Meditation"].ToString().Trim()));
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex.Message);
                    }
                }

                float[] rrArray = (float[])ls.ToArray(typeof(float));
                if (rrArray.Length > 4)
                {
                   // rrArray = Interpolation.Interpolation4(rrArray, 4);

                    //PhyFFT phyfft = new PhyFFT(rrArray, 4f/*freq*/, 128/*samples*/, 0/*K*/, 128 / 2/*L*/, 64 / 2/*D*/, false, 1/*smooth*/, 1/*decimation*/, "Welch", true, true);

                    //lbl_lf.Content = "LF : " + String.Format("{0:0.###}", phyfft.getLF());
                    //lbl_hf.Content = "HF : " + String.Format("{0:0.###}", phyfft.getHF());
                    //lbl_rmssd.Content = "RMSSD : " + String.Format("{0:0.###}", FastStdStats.calculeRMSSD(ls));
                    //lbl_sd1.Content = "SD1 : " + String.Format("{0:0.###}", FastStdStats.calculeSD1(ls));
                    //lbl_sd2.Content = "SD2 : " + String.Format("{0:0.###}", FastStdStats.calculeSD2(ls));
                    //lbl_d2.Content = "D2 : " + String.Format("{0:0.###}", FastStdStats.calculeD2(10, 100, rrArray));
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }



        }

        private async void GetSamplesFromServer()
        {
            
            AlarmMessageBus.log((Brush)this.TryFindResource("GreenColor"), "conectando..."); 
            string data = "?file=" + filename;
            // TODO GetSamplesXML.php "?file=" + filename;
            string url = Protocol.config.AppSettings.Settings["BioSCADA.Server"].Value + "bsweb/services/GetNeurologSamplesXML.php";
            AlarmMessageBus.log((Brush)this.TryFindResource("BlueColor"), "conectando...");
            string response = await AccessTheWebAsync(url + data);
            if (response != "")
            {
                try
                {
                    System.IO.File.WriteAllText(filename, response);
                    ds = new DataSet();
                    ds.ReadXml(filename, XmlReadMode.InferSchema);
                    GetStats(ds);
                    session_panel.FillDataGrig(ds);
                  
                    AlarmMessageBus.log((Brush)this.TryFindResource("GreenColor"),"arquivo atualizado com sucesso");
                }
                catch (Exception ex)
                { 
                    AlarmMessageBus.log((Brush)this.TryFindResource("RedColor"), "erro ao ler arquivo " + ex.Message); 
                }
                
            }
            else
            {
                 
                AlarmMessageBus.log((Brush)this.TryFindResource("RedColor"),"arquivo não existe");
            }
        }

        private async void Send()
        {
            Protocol.acquisition.Size = Protocol.samples.Count;
            string urlr = Protocol.config.AppSettings.Settings["BioSCADA.Server"].Value + "bsweb/services/SendAcquisition.php";
            AlarmMessageBus.log((Brush)this.TryFindResource("BlueColor"), "enviando...");
            string datar = "" + "?id=" + User.ID + "&timestamp=" + Protocol.acquisition.Timestamp + "&file=" + Protocol.acquisition.File + "&size=" + Protocol.acquisition.Size + "&type=" + Protocol.Type;
            string idacquisition = "";
            try
            {
                idacquisition = await AccessTheWebAsync(urlr + datar); 
            }
            catch (Exception ex)
            {
              //  Console.WriteLine(ex.Message);
            }
          
            if (Protocol.IsPlay) {
                sampleCount = Protocol.acquisition.Size;
                string nsamplesperuser = "";
                for (int i = 0; i < Protocol.samples.Count; i++)
                {
                    Sample s = Protocol.samples[Protocol.samples.Count-1];
                    string data = "" + "?idacquisition=" + idacquisition + "&id=" + User.ID + "&type=" + Protocol.acquisition.Type + "&samples=" + s.ToString();
                    // TODO  SendSample.php "" + "?idacquisition=" + idacquisition + "&id=" + User.Id + "&samples=" + s.ToString()
              //      Console.WriteLine(data);
                    string url = Protocol.config.AppSettings.Settings["BioSCADA.Server"].Value + "bsweb/services/SendSample.php";
                    samplePointer = i;
                    AlarmMessageBus.log((Brush)this.TryFindResource("BlueColor"), "enviando " + samplePointer + " de " + Protocol.samples.Count + " ...");

                    try
                    {
                        nsamplesperuser = await AccessTheWebAsync(url + data);
                    }
                    catch (Exception ex)
                    {
                      //  Console.WriteLine(ex.Message);
                    }

                }
                AlarmMessageBus.log((Brush)this.TryFindResource("GreenColor"), "você possuí " + nsamplesperuser + " amostras"); 

            } else { 
               
                sampleCount = Protocol.acquisition.Size;
                string nsamplesperuser = "";
                for (int i = 0; i < sampleCount; i++)
                {
                    Sample s = Protocol.samples[i];
                    string data = "" + "?idacquisition=" + idacquisition + "&id=" + User.ID + "&type=" + Protocol.acquisition.Type + "&samples=" + s.ToString();
                    // TODO  SendSample.php "" + "?idacquisition=" + idacquisition + "&id=" + User.Id + "&samples=" + s.ToString()
                   // Console.WriteLine(data);
                    string url = Protocol.config.AppSettings.Settings["BioSCADA.Server"].Value + "bsweb/services/SendSample.php";
                    samplePointer = i;
                    AlarmMessageBus.log((Brush)this.TryFindResource("BlueColor"), "enviando " + samplePointer + " de " + sampleCount + " ...");
                    try
                    {
                        nsamplesperuser = await AccessTheWebAsync(url + data);
                    }
                    catch (Exception ex)
                    {
                       // Console.WriteLine(ex.Message);
                    }
           
                }
               AlarmMessageBus.log((Brush)this.TryFindResource("GreenColor"), "você possuí " + nsamplesperuser + " amostras");
            
            }
          
        }

        private async void Down()
        {
            string data = "?id=" + User.ID + "&type=" + Protocol.Type;
            string url = Protocol.config.AppSettings.Settings["BioSCADA.Server"].Value + "bsweb/services/GetAcquisitionsXML.php";
            AlarmMessageBus.log((Brush)this.TryFindResource("BlueColor"), "conectando...");
            string response = await AccessTheWebAsync(url + data);
            if (response != "")
            {

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(response);
                XmlNodeList xnList = xml.SelectNodes("/Acquisitions/Acquisition");
                foreach (XmlNode xn in xnList)
                {
                    string timestamp = xn["Timestamp"].InnerText;
                    string id = xn["Id"].InnerText;
                    string filename = xn["File"].InnerText;
                    Acquisition acquisition = new Acquisition(Int32.Parse(id), timestamp, filename, 0, Protocol.Type);
                    Protocol.acquisitions.Add(acquisition);
                }
                Protocol.SaveAcquisitionXML();

                //App app = (App)App.Current;
                //XmlDataProvider xdp = app.TryFindResource("ReportProvider") as XmlDataProvider;
                //if (xdp != null)
                //{

                //    XmlDocument doc = new XmlDocument();
                //    doc.Load("Data/Report.xml");

                //    xdp.Document = doc;
                //    xdp.XPath = "/Acquisitions/Acquisition";
                //    xdp.Refresh();


                //}
                AlarmMessageBus.log((Brush)this.TryFindResource("GreenColor"), "você baixou " + xnList.Count + " aquisições");
                 
            }
            else
            {
                AlarmMessageBus.log((Brush)this.TryFindResource("RedColor"), "não foi possível enviar as amostras, verifique a conexão com a internet");
                 
            }
            
        }

        async Task<string> AccessTheWebAsync(string url)
        {
            HttpClient client = new HttpClient();
            Task<string> getStringTask = client.GetStringAsync(url);
            DoIndependentWork();
            string urlContents = await getStringTask;
            return urlContents;
        }
         

        void DoIndependentWork()
        { 
            AlarmMessageBus.CurrentAlarmColor = (Brush)this.TryFindResource("BlueColor"); 
        }

         
    }
}
