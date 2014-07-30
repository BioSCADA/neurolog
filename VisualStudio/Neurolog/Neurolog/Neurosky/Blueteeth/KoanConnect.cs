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
using BioSCADA;
 
using Neurolog.Blueteeth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace Neurolog.Blueteeth
{
   
    public class KoanConnect
    {
        NeuroskyDriver koanDriver= new NeuroskyDriver();

        // This method will be called when the thread is started.
        public bool DoSend(string port, string device)
        {
            koanDriver.ConfigDriver(port, device);
            if (Protocol.IsConected)
            {
                koanDriver.Close();

                AlarmMessageBus.log((Brush)new BrushConverter().ConvertFrom("#7b0100"), "bluetooth foi desconectado com sucesso!");  

            }
            else
            {
                AlarmMessageBus.log((Brush)new BrushConverter().ConvertFrom("#94bb65"), "conectando dispositivo...");
                koanDriver.Open(); 
            }

            return Protocol.IsConected;
        }

       
 
        public string SendPostAndGetResponse(string url, string postData)
        {
            string webpageContent = "NULL";
  
            return webpageContent; 

        }

       
    }
}
