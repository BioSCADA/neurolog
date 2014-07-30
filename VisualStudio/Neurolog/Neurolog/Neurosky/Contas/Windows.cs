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

namespace Neurolog
{
    class Windows
    {

        static float wsum = 0;
        /* See Oppenheim & Schafer, Digital Signal Processing, p. 241 (1st ed.) */

        static float win_bartlett(int j, int n)
        {
            float a = 2.0F / (n - 1F), w;
            if ((w = j * a) > 1.0F)
                w = 2.0F - w;
            wsum += w;
            return (w);
        }

        /* See Oppenheim & Schafer, Digital Signal Processing, p. 242 (1st ed.) */

        static float win_blackman(int j, int n)
        {
            float a = (2.0F * (float)Math.PI / (n - 1F)), w;
            w = (0.42F - 0.5F * (float)Math.Cos(a * j) + 0.08F * (float)Math.Cos(2F * a * j));
            wsum += w;
            return (w);
        }

        /* See Harris, F.J., "On the use of windows for harmonic analysis with the
           discrete Fourier transform", Proc. IEEE, Jan. 1978 */

        static float win_blackman_harris(int j, int n)
        {
            float a = (2.0f * (float)Math.PI / (n - 1F)), w;
            w = (0.35875F - 0.48829F * (float)Math.Cos(a * j) + 0.14128F * (float)Math.Cos(2 * a * j) - 0.01168F * (float)Math.Cos(3F * a * j));
            wsum += w;
            return (w);
        }

        /* See Oppenheim & Schafer, Digital Signal Processing, p. 242 (1st ed.) */

        static float win_hamming(int j, int n)
        {
            float a = (2.0F * (float)Math.PI / (n - 1F)), w;

            w = (0.54F - 0.46F * (float)Math.Cos(a * j));
            wsum += w;
            return (w);
        }

        /* See Oppenheim & Schafer, Digital Signal Processing, p. 242 (1st ed.)
           The second edition of Numerical Recipes calls this the "Hann" window. */

        static float win_hanning(int j, int n)
        {
            float a = (2.0F * (float)Math.PI / (n - 1F)), w;

            w = (0.5F - 0.5F * (float)Math.Cos(a * j));
            wsum += w;
            return (w);
        }

        /* See Press, Flannery, Teukolsky, & Vetterling, Numerical Recipes in C,
           p. 442 (1st ed.) */

        static float win_parzen(int j, int n)
        {
            float a = ((n - 1) / 2.0F), w;

            if ((w = (j - a) / (a + 1)) > 0.0F)
                w = 1 - w;
            else
                w = 1 + w;
            wsum += w;
            return (w);
        }

        /* See any of the above references. */

        static float win_square(int j, int n)
        {
            wsum += 1.0F;
            return 1.0F;
        }

        /* See Press, Flannery, Teukolsky, & Vetterling, Numerical Recipes in C,
           p. 442 (1st ed.) or p. 554 (2nd ed.) */

        static float win_welch(int j, int n)
        {
            float a = ((n - 1) / 2.0F), w;

            w = (j - a) / (a + 1F);
            w = 1F - w * w;
            wsum += w;
            return (w);
        }

        static String windowType = "";  // defaults to rectangular window

        static void setWindowType(String w)
        {
            if (w.Equals("Dirichlet") || w.Equals("Square"))
                windowType = "SQUARE";
            if (w.Equals("Bartlett"))
                windowType = "BARTLETT";
            if (w.Equals("Hanning"))
                windowType = "HANNING";
            if (w.Equals("Hamming"))
                windowType = "HAMMING";
            if (w.Equals("Blackman"))
                windowType = "BLACKMAN";
            if (w.Equals("Welch"))
                windowType = "WELCH";
            if (w.Equals("Blackman Harris"))
                windowType = "BLACKMAN_HARRIS";
            if (w.Equals("Parzen"))
                windowType = "PARZEN";
        }


        public static float apply(float[] c, int m, String type)
        {
            wsum = 0;

            setWindowType(type);
            for (int i = 0; i < m; i++)
            {
                switch (windowType)
                {
                    case "BARTLETT": // Bartlett (triangular) window
                        c[i] *= win_bartlett(i, m);
                        break;
                    case "WELCH": // WELCH  window
                        c[i] *= win_welch(i, m);
                        break;
                    case "HANNING": // Hanning window
                        c[i] *= win_hanning(i, m);
                        break;
                    case "HAMMING": // Hamming window
                        c[i] *= win_hamming(i, m);
                        break;
                    case "BLACKMAN": // Blackman window
                        c[i] *= win_blackman(i, m);
                        break;
                    case "BLACKMAN_HARRIS": // BLACKMAN_HARRIS window
                        c[i] *= win_blackman_harris(i, m);
                        break;
                    case "PARZEN": // PARZEN window
                        c[i] *= win_parzen(i, m);
                        break;
                    case "SQUARE": // SQUARE window
                        c[i] *= win_square(i, m);
                        break;
                    default:
                        break;// Rectangular window function

                }
            }

            return wsum;

        }

    }
}
