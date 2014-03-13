
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
//                For Open Source Neuro Library  
//
// Copyright (C) 2012-2012, Diego Schmaedech, all rights reserved. 
// Copyright (C) 2012-2012, Paola Barros Delben, all rights reserved. 
 * 
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
using System.ComponentModel;

namespace BioSCADA.Models
{
    /// <summary>
    /// Customer class to be displayed in the property grid
    /// </summary>
    /// 
    [DefaultPropertyAttribute("Name")]
    public class Logging
    {
        
        [CategoryAttribute("Human Settings"), DescriptionAttribute("Name of the human")]
        public string Name{  get; set;  }

        [CategoryAttribute("Human Settings"), DescriptionAttribute("Observation of the human")]
        public string OBS { get; set; }

        [CategoryAttribute("Human Settings"), DescriptionAttribute("Sex of the human")]
        public string Sex { get; set; }

        [CategoryAttribute("Human Settings"), DescriptionAttribute("Timestamp of the human (optional)")]
        public string Timestamp { get; set; }

        [CategoryAttribute("Human Settings"), DescriptionAttribute("Age of the human")]
        public int Age { get; set; }

        [CategoryAttribute("Human Settings"), DescriptionAttribute("Most current e-mail of the human")]
        public string Email { get; set; }

        [CategoryAttribute("Logging Settings"), DescriptionAttribute("Frequency of the simulated wave")]
        public int Frequency { get; set; }

        [CategoryAttribute("Logging Settings"), DescriptionAttribute("Amplitude of the simulated wave")]
        public int Amplitude { get; set; }
        

        [CategoryAttribute("Logging Settings"), DescriptionAttribute("Version")]
        public string Version { get; set; }

        [CategoryAttribute("Logging Settings"), DescriptionAttribute("Error")]
        public string Error { get; set; }

        [CategoryAttribute("Logging Settings"), DescriptionAttribute("PacketsRead")]
        public string PacketsRead { get; set; }

        [CategoryAttribute("Logging Settings"), DescriptionAttribute("Battery")]
        public float Battery { get; set; }

        [CategoryAttribute("Logging Settings"), DescriptionAttribute("PoorSignal")]
        public float PoorSignal { get; set; }

        [CategoryAttribute("Logging Settings"), DescriptionAttribute("Attention")]
        public float Attention { get; set; }

        [CategoryAttribute("Logging Settings"), DescriptionAttribute("Meditation")]
        public float Meditation { get; set; }

        [CategoryAttribute("Logging Settings"), DescriptionAttribute("Raw")]
        public float Raw { get; set; }

        [CategoryAttribute("Logging Settings"), DescriptionAttribute("Delta")]
        public float Delta { get; set; }

        [CategoryAttribute("Logging Settings"), DescriptionAttribute("Theta")]
        public float Theta { get; set; }

        [CategoryAttribute("Logging Settings"), DescriptionAttribute("Alpha1")]
        public float Alpha1 { get; set; }

        [CategoryAttribute("Logging Settings"), DescriptionAttribute("Alpha2")]
        public float Alpha2 { get; set; }

        [CategoryAttribute("Logging Settings"), DescriptionAttribute("Beta1")]
        public float Beta1 { get; set; }

        [CategoryAttribute("Logging Settings"), DescriptionAttribute("Beta2")]
        public float Beta2 { get; set; }

        [CategoryAttribute("Logging Settings"), DescriptionAttribute("Gamma1")]
        public float Gamma1 { get; set; }

        [CategoryAttribute("Logging Settings"), DescriptionAttribute("Gamma2")]
        public float Gamma2 { get; set; }

        [CategoryAttribute("Logging Settings"), DescriptionAttribute("BlinkStrength")]
        public float BlinkStrength { get; set; }

        [CategoryAttribute("Logging Settings"), DescriptionAttribute("LAPCounter")]
        public int LapCounter { get; set; }
        
       
         
        public Logging()
        {
            
        }
    }
}
