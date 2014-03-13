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
using Neurolog.Blueteeth;
using Neurolog.Sessions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using System.Xml;

namespace BioSCADA
{
    /// <summary>
    /// Interaction logic for SessionPanel.xaml
    /// </summary>
    public partial class SessionPanel : UserControl
    {

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        DataSet ds = new DataSet();
        
        public void FillDataGrig(DataSet ds)
        {
            
            try
            {
                dg_session.ItemsSource = ds.Tables[0].DefaultView;
                dg_session.DataContext = ds.Tables[0]; 

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public SessionPanel()
        {
            InitializeComponent();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();


        }
       
        private void bt_save_Click(object sender, RoutedEventArgs e)
        {
             Process.Start(Protocol.RAWFILENAME); 
            //saveFileDialog1.FileName = Protocol.filename;
            //if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{

            //    if (saveFileDialog1.FileName != "")
            //    {
            //        try
            //        {
            //             if (ds.Tables[0].Rows.Count > 0)
            //             {
            //                Protocol.WriteTxT(ds.Tables[0], saveFileDialog1.FileName);
            //                Process.Start(saveFileDialog1.FileName);
            //             }

            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine(ex.Message);
            //        }
                    
            //    }
            //}
        }
    }
}
