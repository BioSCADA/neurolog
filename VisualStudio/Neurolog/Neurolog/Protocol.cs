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
using Neurolog.Sessions;
using MathLibrary;
using System;

using System.Collections;
using System.Configuration; 
using System.Data;
using System.IO;
using System.Windows.Data;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using ZedGraph;
namespace BioSCADA
{
    public class Protocol
    {
        public static float Amplitude = 70;
        public static float Frequency = 60;
        public static float Meditation = 0;
        public static float Attention = 0;
        public static String Timestamp = DateTime.Now.ToLongTimeString(); 
        public static ArrayList wavelist = new ArrayList(); 
        public static float[] waveArray = (float[])wavelist.ToArray(typeof(float));
       

        private static Protocol instance;
        public static Samples samples = new Samples();
        public static Acquisitions acquisitions = new Acquisitions();
        public static Acquisition acquisition;
        public static PointPairList MeditationPairList = new PointPairList();
        public static PointPairList coherencePairList = new PointPairList();
        public static PointPairList fftPowerPairList = new PointPairList();
        public static string Type = "NEUROLOG";
        public static float Battery = 0;
        public static float Raw = 0;
        public static float SimFrequency = 1;
        public static float SimAmplitude = 15;  
        public static float Coherence = 0;
        public static float Alpha1 = 0;
        public static float Beta1 = 0;
        public static float PeakFreq = 0;
        public static string filename = Path.GetDirectoryName(Application.ExecutablePath);
        public static string RAWFILENAME = "[NEUROLOG]" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt";
       
        public static ArrayList Meditationlist = new ArrayList();
        public static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public static string TAGs = "";
        public static float[] MeditationArray = (float[])Meditationlist.ToArray(typeof(float));
        public static bool IsSimuletedRAN = false;
        public static bool IsPlay = false;
        public static bool IsConected = false;
        public static DateTime newAquisitionDate = DateTime.Now;
        public static float LastMeditation { get; set; }
        public static int SampleCount { get; set; }


        public static void RunPhyfft()
        {

            MeditationArray = (float[])Meditationlist.ToArray(typeof(float));
            if (MeditationArray.Length > 4)
            {
                MeditationArray = Interpolation.Interpolation4(MeditationArray, 4);


                PhyFFT phyfft = new PhyFFT(MeditationArray, 4f/*freq*/, 128/*samples*/, 0/*K*/, 128 / 2/*L*/, 64 / 2/*D*/, false, 1/*smooth*/, 1/*decimation*/, "Welch", true, true);

                Coherence = phyfft.getCoherence();
                double x = (double)Protocol.Meditationlist.Count;
                MeditationPairList.Add(x, Meditation);
                coherencePairList.Add(x, Coherence);
            }
        }


        public static void Clear()
        {
            Meditationlist.Clear();
            MeditationPairList.Clear();
            coherencePairList.Clear();
            fftPowerPairList.Clear();
            samples = new Samples();
        }


        public static void AddRAN()
        {
          
            float fs = SimFrequency / 12f;

            float a = SimAmplitude;
            Protocol.Attention = 800 + a * (float)Math.Sin(((float)Protocol.Meditationlist.Count / 1) * ((2f * (float)Math.PI) * fs));
            //  Console.WriteLine(Protocol.rrlist.Count + "\n");
            Protocol.Meditation = (float)Math.Tan(Protocol.Attention);
            DateTime dDate = DateTime.Now;
            Protocol.AddSample(dDate.ToString("yyyy-MM-dd HH:mm:ss.fff"), Protocol.Battery.ToString(), Protocol.Meditation.ToString(), Protocol.Attention.ToString(), Protocol.Alpha1.ToString(), Protocol.Beta1.ToString(), Protocol.Coherence.ToString(), TAGs);
            //Console.WriteLine(Convert.ToInt32(y) + "\n");
            Protocol.SampleCount++;
        }

        public static void AddSample(string p2, string p3, string m, string a, string al1, string bt1, string co, string p7)
        {



            Meditationlist.Add(Meditation);
                RunPhyfft();
                Sample sampling = new Sample(User.ID.ToString(), p2, p3, m.ToString(), a.ToString(), al1.ToString(), bt1.ToString(), co.ToString(), p7);
                samples.Add(sampling);
                LastMeditation = Meditation;
            


        }

        public static void WriteTxT(DataTable dt, string filePath)
        {
            int i = 0;
            StreamWriter sw = null;

            try
            {

                sw = new StreamWriter(filePath, false);

                for (i = 0; i < dt.Columns.Count - 1; i++)
                {

                    sw.Write(dt.Columns[i].ColumnName + (char)9);

                }
                sw.Write(dt.Columns[i].ColumnName);
                sw.WriteLine();

                foreach (DataRow row in dt.Rows)
                {
                    object[] array = row.ItemArray;

                    for (i = 0; i < array.Length - 1; i++)
                    {
                        sw.Write(array[i].ToString() + (char)9);
                    }
                    sw.Write(array[i].ToString());
                    sw.WriteLine();

                }

                sw.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine("Protocol:Operação invalida: \n" + ex.ToString());
            }
        }

        public static void Stop()
        {
            IsPlay = false;
            SaveAcquisitionXML();
            SaveSamplesXML();

        }

        public static void Play()
        {
            IsPlay = true;
            CreateAcquisitionXML();
            CreateSamplesXML();

        }

        public static string CreateAcquisitionXML()
        {
            newAquisitionDate = DateTime.Now;
            Protocol.filename = "[" + User.ID + "]" + newAquisitionDate.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ".xml";

            acquisition = new Acquisition(Int32.Parse(User.ID) , newAquisitionDate.ToString("yyyy-MM-dd HH:mm:ss.fff"), Protocol.filename, 0, Protocol.Type);

            Protocol.acquisitions.Add(acquisition);
            SaveAcquisitionXML();


            return Protocol.filename;
        }

        public static string SaveAcquisitionXML()
        {

            Protocol.acquisitions.CollectionName = "Acquisitions";
            XmlRootAttribute root = new XmlRootAttribute("Acquisitions");
            XmlAttributeOverrides attrOverrides = new XmlAttributeOverrides();
            XmlAttributes attrs = new XmlAttributes();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlSerializer xml = new XmlSerializer(typeof(Acquisitions), root);
            try
            {
                TextWriter writer = new StreamWriter("Data/Report.xml");
                xml.Serialize(writer, Protocol.acquisitions, ns);
                writer.Close();
                App app = (App)App.Current;
                XmlDataProvider xdp = app.TryFindResource("ReportProvider") as XmlDataProvider;
                if (xdp != null)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load("Data/Report.xml");
                    xdp.Document = doc;
                    xdp.XPath = "/Acquisitions/Acquisition";
                    xdp.Refresh();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Protocol.filename;
        }

        public static string CreateSamplesXML()
        {
            Clear();
            SaveSamplesXML();
            return Protocol.filename;
        }

        public static string SaveSamplesXML()
        {

            Protocol.samples.CollectionName = "Samples";
            XmlRootAttribute root = new XmlRootAttribute("Samples");
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlSerializer xml = new XmlSerializer(typeof(Samples), root);

            try
            {
                TextWriter writer = new StreamWriter(Protocol.filename);
                xml.Serialize(writer, Protocol.samples, ns);
                writer.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Protocol.filename;
        }


        private Protocol()
        {

        }

        public static Protocol Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Protocol();
                }
                return instance;
            }
        }

        
    }
}
