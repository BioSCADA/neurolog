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
using Neurolog;
using Neurolog.Blueteeth;
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

namespace BioSCADA
{
    /// <summary>
    /// Interaction logic for ChartPanel.xaml
    /// </summary>
    public partial class ChartPanel : UserControl
    {
        Chart chartPanel;
        int lastSampleCount = 0;
        public ChartPanel()
        {
            InitializeComponent();
            chartPanel = new Chart();
            wfh_chart.Child = chartPanel;
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            dispatcherTimer.Start();

             
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (lastSampleCount < Protocol.SampleCount) { chartPanel.updatePanel(); } else { }
           

            lastSampleCount = Protocol.SampleCount;
        }
         

        private void s_freq_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //Protocol.SimFrequency = (float)s_freq.Value;
        }

        private void s_ampl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            //Protocol.SimAmplitude = (float)s_ampl.Value;
        }

        private void bt_add_tag_Click(object sender, RoutedEventArgs e)
        {
            Protocol.TAGs = txt_tag.Text;
            AlarmMessageBus.log((Brush)this.TryFindResource("GreenColor"), Protocol.TAGs );
               
        }
    }
}
