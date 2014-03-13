using BioSCADA;
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
using Neurolog.Blueteeth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Neurolog.Sessions
{
    class Worker
    {
        // HTIS IS A EXAMPLES
        public string DoSend()
        {
            string response = "NULL";
            foreach (Sample s in Protocol.samples)
            {
                string data = "" + "?action=add&login=" + User.Login + "&samples=" + s.ToString();
                string url = Protocol.config.AppSettings.Settings["BioSCADA.Server"].Value + "BioSCADARequest.php";
                response = SendPostAndGetResponse(url, data);

            }
            return response;
        }

        public string SendPostAndGetResponse(string url, string postData)
        {
            string webpageContent = "NULL";

            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(url + postData);
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url + postData);
                webRequest.Method = "POST";
                webRequest.ContentType = "text/xml;charset=utf-8";
                webRequest.ContentLength = byteArray.Length;
                using (Stream webpageStream = webRequest.GetRequestStream())
                {
                    webpageStream.Write(byteArray, 0, byteArray.Length);
                    webpageStream.Close();
                }

                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    if (webResponse.StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                        {
                            webpageContent = reader.ReadToEnd();
                            reader.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                AlarmMessageBus.log((System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#7b0100"), "falhou envio para núvem! ");
            }

            return webpageContent;
        }


    }
}
