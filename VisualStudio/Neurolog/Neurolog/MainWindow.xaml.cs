/*M///////////////////////////////////////////////////////////////////////////////////////
//
//  IMPORTANT: READ BEFORE DOWNLOADING, COPYING, INSTALLING OR USING.
//
//  By downloading, copying, installing or using the software you agree to this license.
//  If you do not agree to this license, do not download, install,
//  copy or use the software.
//
//
//                           BioSCADA® License Agreement
//                For Open Source Human SCADA Library  
//
// Copyright (C) 2011-2014, Diego Schmaedech for this and Many Others Developers around the worlds for all, all rights reserved. 
//
//							For Open Source Human SCADA aplications
//
// Copyright (C) 2011-2014, Prof. Dr. Emílio Takase, Laboratório de Educação Cerebral, all rights reserved.
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
using System.Collections.ObjectModel;
using System.Dynamic;
using Selen.Wpf.SystemStyles;
using Neurolog.Blueteeth;
using Neurolog.Sessions;
using System.IO.Ports;
using Neurolog;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
namespace BioSCADA
{
     
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        TCPDriver tcpDriver = new TCPDriver();

        public MainWindow()
        {

           
            DateTime date1 = new DateTime(2015, 1, 1, 2, 14, 14);
            DateTime date2 = DateTime.Now;

            int result = DateTime.Compare(date1, date2);
            Console.WriteLine(result);
            if (result < 0)
            {
                Environment.Exit(0);
            }
            else {
                InitializeComponent();


                System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Start();


                autoConnectTCP();
            
            }


               
 
        }



        public void autoConnectTCP()
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            if (tcpDriver.startListening(9999))
            { 
                AlarmMessageBus.log((Brush)this.TryFindResource("GreenColor"), "você está online!"); 
            }
            else
            { 
                AlarmMessageBus.log((Brush)this.TryFindResource("GreenColor"), "não foi possível estabelecer conexão!");  
            }

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Protocol.Timestamp = DateTime.Now.ToLongTimeString();
            if (Protocol.IsSimuletedRAN && Protocol.IsPlay)
            {
                Protocol.AddRAN(); 
            }
            if (User.IsLogged) 
            {
                txt_user.Text = User.Login; 
            } 
            else 
            {
                
            }
            if (Protocol.IsConected)
            {
                panel_device.bt_connect.Content = "Conectado!";
                panel_device.bt_connect.Background = (Brush)this.TryFindResource("BackgroundSelected");
            }
            else
            {
                panel_device.bt_connect.Content = "Conectar";
                panel_device.bt_connect.Background = (Brush)this.TryFindResource("BackgroundNormal");
            }
            if (Protocol.IsPlay)
            {
                bt_play.Background = (Brush)this.FindResource("BackgroundSelected");
              
            }
            else
            {
                bt_play.Background = (Brush)this.FindResource("BackgroundNormal");
            }
            txt_alarm.Text = AlarmMessageBus.CurrentAlarmMessage;
            sb_message_bus.Background = AlarmMessageBus.CurrentAlarmColor; 
        }
  

        private void cmbState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem)cmbState.SelectedItem;
            if (comboBoxItem.Content.ToString() == "Monitoramento")
            {

                AlarmMessageBus.log((Brush)this.TryFindResource("GreenColor"), "monitoramento"); 
                //  tbAmplitude.Visible = false;
              //  Console.WriteLine("MainWindow:Monitoramento"); 
                //   tbFrequency.Visible = false;
                Protocol.IsSimuletedRAN = false;
                //   this.tsbSimulated.Image = global::Koan.Properties.Resources.Loop1;
            }
            else
            {
                AlarmMessageBus.log((Brush)this.TryFindResource("GreenColor"), "simulação"); 
               // Console.WriteLine("MainWindow:Simulated"); 
                //   tbAmplitude.Visible = true;
                //    this.tsbSimulated.Image = global::Koan.Properties.Resources.Loop2;
                //   tbFrequency.Visible = true;
                Protocol.IsSimuletedRAN = true;
            }

        }
  
        private void bt_play_Click(object sender, RoutedEventArgs e)
        {
            if (!Protocol.IsConected && !Protocol.IsSimuletedRAN)
            {
                panel_device.Connect();
            }
            AlarmMessageBus.log((Brush)this.TryFindResource("GreenColor"), "sessão iniciada!");  
            bt_play.Background = (Brush)this.FindResource("BackgroundSelected");
            bt_stop.Background = (Brush)this.FindResource("BackgroundNormal");
            Protocol.Play(); 
        }
         

        private void bt_stop_Click(object sender, RoutedEventArgs e)
        {
            if (Protocol.IsConected)
            {
                panel_device.Connect();
            }
            
            AlarmMessageBus.log((Brush)this.TryFindResource("GreenColor"), "sessão fechada!");  
            bt_play.Background = (Brush)this.FindResource("BackgroundNormal");
            bt_stop.Background = (Brush)this.FindResource("BackgroundSelected");
            Protocol.Stop(); 
        }

        private void bt_up_Click(object sender, RoutedEventArgs e)
        {
            report_panel.bt_up_Click( sender, e); 
        }
         
         
    }
}