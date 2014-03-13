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

using System;
using System.IO.Ports;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Threading;
using Neurolog.Sessions;
using BioSCADA;
using ThinkGearNET;
using BioSCADA.Models;
using System.Windows.Media;
using System.Text;

namespace Neurolog.Blueteeth
{
    public class NeuroskyDriver
    {
        private ThinkGearWrapper _thinkGearWrapper = new ThinkGearWrapper();
        
        Logging logging = new Logging();
        private string device = "";
        private string port = "";
        TextWriter file = new StreamWriter(Protocol.RAWFILENAME);
        string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        
        private void CreateHeader()
        {
          
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Timestamp{0};", "");
            sb.AppendFormat("Version{0};", "");
            sb.AppendFormat("Error{0};", "");
            sb.AppendFormat("PacketsRead{0};", "");
            sb.AppendFormat("Battery{0};", "");
            sb.AppendFormat("PoorSignal{0};", "");
            sb.AppendFormat("Attention{0};", "");
            sb.AppendFormat("Meditation{0};", "");
            sb.AppendFormat("Raw{0};", "");
            sb.AppendFormat("Delta{0};", "");
            sb.AppendFormat("Theta{0};", "");
            sb.AppendFormat("Alpha1{0};", "");
            sb.AppendFormat("Alpha2{0};", "");
            sb.AppendFormat("Beta1{0};", "");
            sb.AppendFormat("Beta2{0};", "");
            sb.AppendFormat("Gamma1{0};", "");
            sb.AppendFormat("Gamma2{0};", "");
            sb.AppendFormat("BlinkStrength{0};", "");
            sb.AppendFormat("LapCounter{0};", "");
            file.WriteLine(sb.ToString());
            file.Flush();
            file.WriteLine();
            file.Flush();
           // Console.WriteLine(sb.ToString());
        }
        public void ConfigDriver(string port, string device)
        {
            this.port = port;
            this.device = device;
            AlarmMessageBus.log((Brush)new BrushConverter().ConvertFrom("#94bb65"), "");
        }
        public NeuroskyDriver( )
        {
            CreateHeader();
            AlarmMessageBus.log((Brush)new BrushConverter().ConvertFrom("#94bb65"), ""); 
        }

        void _thinkGearWrapper_ThinkGearChanged(object sender, ThinkGearChangedEventArgs e)
        {
            Protocol.IsConected = true;  
            logging.Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"); 
            logging.Battery = e.ThinkGearState.Battery;
            logging.PoorSignal = e.ThinkGearState.PoorSignal;
            logging.Attention = e.ThinkGearState.Attention;
            logging.Meditation = e.ThinkGearState.Meditation; 
            logging.Raw = e.ThinkGearState.Raw;
            logging.Delta = e.ThinkGearState.Delta;
            logging.Theta = e.ThinkGearState.Theta;
            logging.Alpha1 = e.ThinkGearState.Alpha1;
            logging.Alpha2 = e.ThinkGearState.Alpha2;
            logging.Beta1 = e.ThinkGearState.Beta1;
            logging.Beta2 = e.ThinkGearState.Beta2;
            logging.Gamma1 = e.ThinkGearState.Gamma1;
            logging.Gamma2 = e.ThinkGearState.Gamma2;
            logging.BlinkStrength = e.ThinkGearState.BlinkStrength;
            logging.LapCounter = 0;
            Protocol.Raw = logging.Raw;
            Protocol.SampleCount++;
            if (Protocol.IsPlay)
            {
                Protocol.Attention = logging.Attention;
                Protocol.Meditation = logging.Meditation;
                Protocol.Alpha1 = logging.Alpha1;
                Protocol.Beta1 = logging.Beta1;
                Protocol.Battery = logging.Battery;
                file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ";" + e.ThinkGearState.ToString() + ";" + Protocol.TAGs);
                file.Flush();
                Protocol.AddSample(logging.Timestamp, Protocol.Battery.ToString(), Protocol.Meditation.ToString(), Protocol.Attention.ToString(), Protocol.Alpha1.ToString(), Protocol.Beta1.ToString(), Protocol.Coherence.ToString(), Protocol.TAGs.ToString());
            }
            else
            { 
            }
           // Console.WriteLine(logging.Raw);
            Thread.Sleep(10);
        } 

        public void Open()
        { 
            try
            { 
               
                if (_thinkGearWrapper == null)
                {
                    Protocol.IsPlay = false;
                    Protocol.IsConected = false;
                    AlarmMessageBus.log((Brush)new BrushConverter().ConvertFrom("#7b0100"), "não foi possível conectar com o NeuroSky! " + port);
                }
                else
                { 
                    _thinkGearWrapper.ThinkGearChanged += _thinkGearWrapper_ThinkGearChanged; 
                    if (!_thinkGearWrapper.Connect(port, 57600, true, false))
                    {
                        Protocol.IsPlay = false;
                        Protocol.IsConected = false;
                        AlarmMessageBus.log((Brush)new BrushConverter().ConvertFrom("#7b0100"), "não foi possível conectar com o NeuroSky! " + port);
                    }
                    else
                    {
                        AlarmMessageBus.log((Brush)new BrushConverter().ConvertFrom("#94bb65"), "NeuroSky conectado! Clique em Play para iniciar a sessão. "); 
                        _thinkGearWrapper.EnableBlinkDetection(true);

                    }
                } 
            }
            catch (Exception ex)
            {
                Protocol.IsPlay = false;
                Protocol.IsConected = false;
                AlarmMessageBus.log((Brush)new BrushConverter().ConvertFrom("#7b0100"), "não foi possível conectar com o NeuroSky! " + port);
            } 
        }

        public void Close()
        {
            _thinkGearWrapper.Disconnect();
            Protocol.IsPlay = false;
            Protocol.IsConected = false;
        }


    }
}
