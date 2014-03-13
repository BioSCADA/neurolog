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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neurolog
{
    class FrequencyDomain
    {

        static float TWOPI = (2.0f * (float) Math.PI);

        public static float calculeVLF(float[] xs, float[] ys)
        {
            float result = 0;
            int i = 0;

            foreach (float f in xs)
            {
                if (f <= 0.04f)
                {
                    result += ys[i];
                }
                i++;
            }

            return result;
        }

        public static float calculeLF(float[] xs, float[] ys)
        {
            float result = 0;
            int i = 0;
            foreach (float f in xs)
            {
                if (f > 0.04f && f <= 0.15f)
                {
                    result += ys[i];
                }
                i++;
            }
            return result;
        }

        public static float calculeHF(float[] xs, float[] ys)
        {
            float result = 0;
            int i = 0;
            foreach (float f in xs)
            {
                if (f > 0.15f && f <= 0.4f)
                {
                    result += ys[i];
                }
                i++;
            }
            return result;
        }

        public static float calculeLFHF(float[] xs, float[] ys)
        {
            return calculeLF(xs, ys) / calculeHF(xs, ys);
        }

        public static void realft(float[] data, int n, int isign)
        {
            //    for (int h = 0; h < n; h++){
            //	    System.out.println( "h: "+ h + "\t"+ data[h]);
            //        }
            int i, i1, i2, i3, i4, n2p3;
            float c1 = 0.5F, c2, h1r, h1i, h2r, h2i;
            float wr, wi, wpr, wpi, wtemp, theta;


            theta = ((float)Math.PI / (float)n);
            if (isign == 1)
            {
                c2 = -0.5F;
                four1(data, n, 1);
            }
            else
            {
                c2 = 0.5F;
                theta = -theta;
            }
            wtemp = (float)Math.Sin(0.5 * theta);
            wpr = (-2.0F * wtemp * wtemp);
            wpi = (float)Math.Sin(theta);
            wr = (1.0F + wpr);
            wi = wpi;
            n2p3 = 2 * n + 3;
            for (i = 2; i <= n / 2; i++)
            {
                try
                {
                    i4 = 1 + (i3 = n2p3 - (i2 = 1 + (i1 = i + i - 1)));
                    h1r = c1 * (data[i1] + data[i3]);
                    h1i = c1 * (data[i2] - data[i4]);
                    h2r = -c2 * (data[i2] + data[i4]);
                    h2i = c2 * (data[i1] - data[i3]);
                    data[i1] = (h1r + wr * h2r - wi * h2i);
                    data[i2] = (h1i + wr * h2i + wi * h2r);
                    data[i3] = (h1r - wr * h2r + wi * h2i);
                    data[i4] = (-h1i + wr * h2i + wi * h2r);
                    wr = (wtemp = wr) * wpr - wi * wpi + wr;
                    wi = wi * wpr + wtemp * wpi + wi;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            if (isign == 1)
            {
                data[1] = (h1r = data[1]) + data[2];
                data[2] = h1r - data[2];
            }
            else
            {
                data[1] = c1 * ((h1r = data[1]) + data[2]);
                data[2] = c1 * (h1r - data[2]);
                four1(data, n, -1);
            }
        }

        public static void four1(float[] data, int nn, int isign)
        {


            int nf, mmax, mf, j, istep, i;
            float wtemp, wr, wpr, wpi, wi, theta;
            float tempr, tempi;

            nf = nn << 1;
            j = 1;
            for (i = 1; i < nf; i += 2)
            {
                if (j > i)
                {
                    tempr = data[j];
                    data[j] = data[i];
                    data[i] = tempr;
                    tempr = data[j + 1];
                    data[j + 1] = data[i + 1];
                    data[i + 1] = tempr;
                }
                mf = nf >> 1;
                while (mf >= 2 && j > mf)
                {
                    j -= mf;
                    mf >>= 1;
                }
                j += mf;
            }
            mmax = 2;
            while (nf > mmax)
            {
                istep = 2 * mmax;
                theta = TWOPI / (isign * mmax);
                wtemp = (float)Math.Sin(0.5F * theta);
                wpr = (-2.0F * wtemp * wtemp);
                wpi = (float)Math.Sin(theta);
                wr = 1.0F;
                wi = 0.0F;
                for (mf = 1; mf < mmax; mf += 2)
                {
                    for (i = mf; i <= nf; i += istep)
                    {
                        j = i + mmax;

                        try
                        {
                            tempr = (wr * data[j] - wi * data[j + 1]);
                            tempi = (wr * data[j + 1] + wi * data[j]);
                            data[j] = data[i] - tempr;
                            data[j + 1] = data[i + 1] - tempi;
                            data[i] += tempr;
                            data[i + 1] += tempi;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    wr = (wtemp = wr) * wpr - wi * wpi + wr;
                    wi = wi * wpr + wtemp * wpi + wi;
                }
                mmax = istep;
            }
        }

    }
}
