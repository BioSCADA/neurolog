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
using System.Threading.Tasks;

namespace MathLibrary
{
    public static class Interpolation
    {

        public static float LagrangeInterpolation(float[] x, float[] y, float xval)
        {
            float yval = 0.0f;
            float Products = y[0];
            for (int i = 0; i < x.Length; i++)
            {
                Products = y[i];
                for (int j = 0; j < x.Length; j++)
                {
                    if (i != j)
                    {
                        Products *= (xval - x[j]) / (x[i] - x[j]);
                    }
                }
                yval += Products;
            }
            return yval;
        }

        public static float[] LagrangeInterpolation(float[] x, float[] y, float[] xvals)
        {
            float[] yvals = new float[xvals.Length];
            for (int i = 0; i < xvals.Length; i++)
                yvals[i] = LagrangeInterpolation(x, y, xvals[i]);
            return yvals;
        }

        public static float BarycentricInterpolation(float[] x, float[] y, float xval)
        {
            float product;
            float deltaX;
            float bc1 = 0;
            float bc2 = 0;
            int size = x.Length;
            float[] weights = new float[size];
            for (int i = 0; i < size; i++)
            {
                product = 1;
                for (int j = 0; j < size; j++)
                {
                    if (i != j)
                    {
                        product *= (x[i] - x[j]);
                        weights[i] = 1.0f / product;
                    }
                }
            }
            for (int i = 0; i < size; i++)
            {
                deltaX = weights[i] / (xval - x[i]);
                bc1 += y[i] * deltaX;
                bc2 += deltaX;
            }
            return bc1 / bc2;
        }

        public static float[] Interpolation4(float[] ydata, int factor)
        {

            float[] xdata = new float[ydata.Length];
            float[] x = new float[(ydata.Length * factor) - factor];
            for (int i = 0; i < ydata.Length; i++)
            {
                xdata[i] = i + 1;
            }
            x[0] = 1;
            for (int i = 1; i < x.Length; i++)
            {
                x[i] = x[i - 1] + (float)(ydata.Length) / (float)(xdata.Length * factor);
            }
            // float[] x = new float[] { 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6, 6.5, 7, 7.5, 8, 8.5, 9, 9.5, 10 };
            // float[] y = Interpolation.LagrangeInterpolation(xdata, ydata, x);
            // float[] y2 = Interpolation.BarycentricInterpolation(xdata, ydata, x);
            float[] y3 = Interpolation.LinearInterpolation(xdata, ydata, x);
            //for (int i = 0; i < x.Length; i++)
            //{
            //    if (i < xdata.Length)
            //    {
            //        Console.WriteLine(xdata[i] + "\t" + ydata[i] + "\t" + x[i] + "\t" + y3[i]);
            //    }
            //    else
            //    {
            //        Console.WriteLine(" " + "\t" + " " + "\t" + x[i] + "\t" + y3[i]);
            //    }
            //}
            return y3;
        }

        public static float[] BarycentricInterpolation(float[] x, float[] y, float[] xvals)
        {
            float[] yvals = new float[xvals.Length];
            for (int i = 0; i < xvals.Length; i++)
                yvals[i] = BarycentricInterpolation(x, y, xvals[i]);
            return yvals;
        }

        public static float LinearInterpolation(float[] x, float[] y, float xval)
        {
            float yval = 0.0f;
            for (int i = 0; i < x.Length - 1; i++)
            {
                if (xval >= x[i] && xval < x[i + 1])
                {
                    yval = y[i] + (xval - x[i]) * (y[i + 1] - y[i]) / (x[i + 1] - x[i]);
                }
            }
            return yval;
        }
        public static float[] LinearInterpolation(float[] x, float[] y, float[] xvals)
        {
            float[] yvals = new float[xvals.Length];
            for (int i = 0; i < xvals.Length; i++)
                yvals[i] = LinearInterpolation(x, y, xvals[i]);
            return yvals;
        }
    }
}
