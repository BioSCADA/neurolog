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
//                For Open Source Heart Rate SCADA Library  
//
// Copyright (C) 2011-2012, Diego Schmaedech, all rights reserved. 
//
							For Open Source Biosignal SCADA
//
// Copyright (C) 2012, Laboratório de Educação Cerebral, all rights reserved.
//
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
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Neurolog.Sessions;
using System.Configuration;
using System.Net.Http;
using System.Xml;
namespace BioSCADA
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserLogin : UserControl
    {

        public UserLogin()
        {
            InitializeComponent();
        }

        private void bt_login_Click(object sender, RoutedEventArgs e)
        {
            if (bt_login.Content == "Sair")
            {
                User.IsLogged = true;
                User.Login = "";
                User.DecryptedPass = "";
                bt_login.Content = "Login";
            }
            else
            {
                GetConnected(txt_login.Text, txt_pass.Password);
            }


        }

        private async void GetConnected(string login, string pass)
        {

            string EncryptedUsername = EncryptManager.EncryptString(login, EncryptManager.Key);
            string EncryptedPass = EncryptManager.EncryptString(pass, EncryptManager.Key);
            Protocol.config.AppSettings.Settings["BioSCADA.UserName"].Value = EncryptedUsername;
            Protocol.config.AppSettings.Settings["BioSCADA.Password"].Value = EncryptedPass;
            Protocol.config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            Console.WriteLine("BioSCADA.UserName = " + Protocol.config.AppSettings.Settings["BioSCADA.UserName"].Value);
            Console.WriteLine("BioSCADA.Password = " + Protocol.config.AppSettings.Settings["BioSCADA.Password"].Value);


            /*http://server.bioscada.com.br/BioSCADARequest.php?action=login&login=admin&pass=lec$admin*/

            string url = Protocol.config.AppSettings.Settings["BioSCADA.Server"].Value + "crypta.php?action=getperson&" + "login=" + login + "&pass=" + pass;
            AlarmMessageBus.log((Brush)this.TryFindResource("BlueColor"), "conectando...");
            string response = "";
            try { response = await AccessTheWebAsync(url); }
            catch (Exception ex) { }
            if (response.Trim().StartsWith("<?xml") && response != "")
            {
                User.IsLogged = true;
                User.Login = login;
                User.DecryptedPass = pass;

                System.IO.File.WriteAllText("Data/People.xml", response);
                XmlDataProvider xdp = this.TryFindResource("PeopleProvider") as XmlDataProvider;
                if (xdp != null)
                {

                    XmlDocument doc = new XmlDocument();
                    doc.Load("Data/People.xml");
                    xdp.Document = doc;
                    xdp.XPath = "/People/Person";
                    xdp.Refresh();
                    XmlElement ndElement = doc.DocumentElement;
                    foreach (XmlNode node in ndElement)
                    {
                        Console.WriteLine("User.Id " + node.Attributes["Id"].Value);
                        User.ID = node.Attributes["Id"].Value;
                    }

                }

                bt_login.Content = "Sair";
                AlarmMessageBus.log((Brush)this.TryFindResource("GreenColor"), login + " ID " + User.ID + " está online.");
            }
            else
            {
                User.IsLogged = false;
                AlarmMessageBus.log((Brush)this.TryFindResource("RedColor"), login + " usuário não encontrado! ");
            }

        }

        async Task<string> AccessTheWebAsync(string url)
        {
            // You need to add a reference to System.Net.Http to declare client.
            HttpClient client = new HttpClient();

            // GetStringAsync returns a Task<string>. That means that when you await the
            // task you'll get a string (urlContents).
            Task<string> getStringTask = client.GetStringAsync(url);

            // You can do work here that doesn't rely on the string from GetStringAsync.
            DoIndependentWork();

            // The await operator suspends AccessTheWebAsync.
            //  - AccessTheWebAsync can't continue until getStringTask is complete.
            //  - Meanwhile, control returns to the caller of AccessTheWebAsync.
            //  - Control resumes here when getStringTask is complete. 
            //  - The await operator then retrieves the string result from getStringTask.
            string urlContents = await getStringTask;

            // The return statement specifies an integer result.
            // Any methods that are awaiting AccessTheWebAsync retrieve the length value.
            return urlContents;
        }

        void DoIndependentWork()
        {
            bt_login.Content = "...";
            AlarmMessageBus.CurrentAlarmColor = (Brush)this.TryFindResource("BlueColor");
        }


    }
}
