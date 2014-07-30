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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neurolog
{
    class FastStdStats
    {



        public static float[] getFloatArray(ArrayList al)
        { 
            return (float[])al.ToArray(typeof(float));
        }

        public static float[] getFloatArrayFromString(ArrayList vector)
        {
            float[] values = new float[vector.Count];
            for (int i = 0; i < vector.Count; i++)
            {
                values[i] = (float)vector[i];
            }
            return values;
        }
 
        public static float calculeMEAN(ArrayList rr)
        {
            return Statistics.mean(getFloatArray(rr));
        }

        public static float calculeSD(ArrayList rr)
        {
            return Statistics.stddev(getFloatArray(rr));
        }

        public static float calculeSD1(ArrayList rr)
        {
            return (float)(Math.Sqrt(0.5) * calculeSDSD(rr));
        }

        public static float calculeSD2(ArrayList rr)
        {
            return (float)Math.Sqrt(2 * Math.Pow(calculeSD(rr), 2) - 0.5 * Math.Pow(calculeSDSD(rr), 2));
        }

        public static float calculeSDSD(ArrayList rr)
        {
            float[] adjacent = new float[rr.Count - 1];
            for (int t = 0; t < rr.Count - 1; t++)
            {
                adjacent[t] = (float)rr[t] - (float)rr[t + 1];
            }
            return Statistics.stddev(adjacent);
        }

        public static float calculeRMSSD(ArrayList rr)
        {
            float[] adjacent = new float[rr.Count - 1];
            float[] adjacentPow2 = new float[adjacent.Length];

            for (int t = 0; t < rr.Count - 1; t++)
            {
                adjacent[t] = (float)rr[t] - (float)rr[t + 1];
                adjacentPow2[t] = (float)Math.Pow(adjacent[t], 2);
            }
            float adj = 0;
            for (int t = 0; t < adjacentPow2.Length; t++)
            {
                adj += adjacentPow2[t];
            }
            return (float)Math.Sqrt(adj / adjacentPow2.Length);
        }

        /*
         * y = ax + b where result[0]=a and result[1] = b
         * and correlation r = result[2]
         */
        public static float[] calculeLinearRegression(float[] x, float[] y)
        {
            float[] result = new float[] { 0f, 0f, 0f };
            int n = x.Length;
            float sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0, sumY2 = 0;
            sumX = Statistics.sum(x);
            sumY = Statistics.sum(y);
            for (int i = 0; i < n; i++)
            {
                sumXY += x[i] * y[i];
                sumX2 += (float)Math.Pow(x[i], 2);
                sumY2 += (float)Math.Pow(y[i], 2);
            }



            try
            {
                result[0] = (n * sumXY - sumX * sumY) / (n * sumX2 - (float)Math.Pow(sumX, 2));
                //result[1] = (sumY-result[0]*sumX)/n;
                //result[2] = ( n*sumXY-sumX*sumY )/( (float)Math.sqrt( (n*sumX2-(float)Math.pow(sumX, 2))*(n*sumY2-Math.pow(sumY,2)) ) );
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new float[] { 0f, 0f, 0f };
            }
            return result;
        }

        /* NON-LINEAR methods
        * m, specifies the pattern length
        * r, defines the criterion of similarity
        * called by chart engine
        */
        public static float calculeApEn(int m, float r, ArrayList rr)
        {
            float result = 0;
            result = calculePhy(m, r, rr) - calculePhy(m + 1, r, rr);
            return result;
        }

        //variavel para o calculo da entropia aproximada //somatorio do Cm
        private static float calculePhy(int m, float r, ArrayList rr)
        {
            float result = 0;
            int N = rr.Count;
            int jNm = N - m + 1;
            float[][] Uuj = new float[jNm][];
            for (int k = 0; k < jNm; k++)
            {
                for (int i = 0; i < m; i++)
                {
                    Uuj[k][ i] = (float)rr[i + k];
                }
            }
            for (int i = 0; i < jNm; i++)
            {
                result += (float)Math.Log(calculeCmj(i, Uuj, r, jNm, true));
            }
            result = result / (float)jNm;
            return result;
        }

        /*
         * called by chart engine
         */
        public static float calculeLnSampEn(int m, float r, ArrayList rr)
        {
            float result = 0;
            try
            {
                result = (float)Math.Log((calculeSampEn(m, r, rr) / calculeSampEn(m + 1, r, rr)));
            }
            catch (Exception ex)
            {
                ex.ToString();
                //Exceptions.printStackTrace(ex);
            }
            return result;
        }

        //calculo somatorio do Cm
        private static float calculeSampEn(int m, float r, ArrayList rr)
        {
            float result = 0;
            int N = rr.Count;
            int jNm = N - m + 1;
            float[][] Uuj = new float[jNm][];
            for (int k = 0; k < jNm; k++)
            {
                for (int i = 0; i < m; i++)
                {
                    Uuj[k][ i] = (float)rr[i + k];
                }
            }
            for (int i = 0; i < jNm; i++)
            {
                result += calculeCmj(i, Uuj, r, jNm, false);
            }
            result = result / (float)jNm;
            return result;
        }

        public static float calculeD2(int m, int rBin, float[] rr)
        {
            if (rr.Length > m)
            {
                ArrayList slopeX = new ArrayList();
                ArrayList slopeY = new ArrayList();
                float[] x = new float[rBin];
                float[] y = new float[rBin];
                float r = -1.5f;
                float lnD2 = 0f;
                for (int i = 0; i < rBin; i++)
                {
                    r += 0.015f;
                    x[i] = r;

                    try
                    {
                        lnD2 = calculeLnD2(m, (float)Math.Exp(r + 1f), rr);
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                        return 0f;
                    }
                    y[i] = lnD2;
                }

                if (y.Length > 0)
                {
                    float middle = Statistics.max(y) / 3f;
                    float middle2 = middle * 2f;
                    for (int i = 0; i < rBin; i++)
                    {
                        if (y[i] > middle && y[i] < middle2)
                        {
                            slopeX.Add(x[i]);
                            slopeY.Add(y[i]);
                        }
                    }

                    if (slopeX != null && slopeX.Count > 0 && slopeY != null && slopeY.Count > 0)
                    {
                        return calculeLinearRegression(getFloatArray(slopeX), getFloatArray(slopeY))[0];
                    }
                    else
                    {
                        return 0f;
                    }
                }
                else
                {
                    return 0f;
                }
            }
            else
            {
                return 0f;
            }
        }

        /*
         * double lnr = -1.5;
         *   for(int i = 0; i < 50; i++){
         *        lnr += 0.02;
         *        System.out.println( lnr +"\t"+ StdStats.calculeLnD2(10, Math.exp(lnr), rr) );
         *   }
         */
        public static float calculeLnD2(int m, float r, float[] rr)
        {
            float result = 0;

            int jNm = rr.Length - m + 1;
            float[][] Uuj = new float[jNm][];
            for (int k = 0; k < jNm; k++)
            {
                for (int i = 0; i < m; i++)
                {
                    Uuj[k][ i] = rr[i + k];
                }
            }
            for (int i = 0; i < jNm; i++)
            {
                result += (float)calculeD2Cmj(i, Uuj, r, jNm, false);
            }

            return (float)Math.Log(result / (float)jNm);
        }

        private static double calculeD2Cmj(int j, float[][] Uj, float r, int jNm, bool itself)
        {
            float result = 0;
            int nbrofu = 0;
            for (int k = 0; k < jNm; k++)
            {
                if (itself)
                {
                   
                    result = dCoUjUk(Uj[j], Uj[k]);
                }
                else
                {
                    if (j != k)
                    {
                        result = dCoUjUk(Uj[j], Uj[k]);
                    }
                }
                if (result <= r)
                {
                    nbrofu++;
                }
            }
            if (itself)
            {
                result = (float)nbrofu / jNm;
            }
            else
            {
                result = (float)nbrofu / (jNm - 1);
            }
            return result;
        }

        private static float calculeCmj(int j, float[][] Uj, float r, int jNm, bool itself)
        {
            float result = 0;
            int nbrofu = 0;
            for (int k = 0; k < jNm; k++)
            {
                if (itself)
                {
                    result = dEnUjUk(Uj[j], Uj[k]);
                }
                else
                {
                    if (j != k)
                    {
                        result = dEnUjUk(Uj[j], Uj[k]);
                    }
                }

                if (result <= r)
                {
                    nbrofu++;
                }
            }
            if (itself)
            {
                result = (float)nbrofu / jNm;
            }
            else
            {
                result = (float)nbrofu / (jNm - 1);
            }

            return result;
        }

        //definido para ApEn e SamplEn
        private static float dEnUjUk(float[] uj, float[] uk)
        {
            float[] result = new float[uj.Length];
            int i = 0;
            foreach (float f in uj)
            {
                result[i] = Math.Abs(uk[i] - f);
                i++;
            }

            return Statistics.max(result);
        }

        //definido para dimension correlation
        private static float dCoUjUk(float[] uj, float[] uk)
        {
            float result = 0f;
            int i = 0;
            foreach (float f in uj)
            {
                result += (float)Math.Pow(uk[i] - f, 2);
                i++;
            }

            return (float)Math.Sqrt(result);
        }




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

    }
}
