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
    class Statistics
    {

        
    public static float max(float[] a) {
        float max = float.NegativeInfinity;
        foreach (float f in a) {
            if (f > max) max = f;
        }
        return max;
    }
 
    /**
    * Return minimum value in array, +infinity if no such value.
    */
    public static float min(float[] a) {
        float min = float.PositiveInfinity;
        foreach (float f in a) {
            if (f < min) min = f;
        }
        return min;
    }

    /**
    * Return average value in array, NaN if no such value.
    */
    public static float mean(float[] a) {
        return sum(a) / a.Length;
    }

    
    /**
    * Return sample variance of array, NaN if no such value.
    */
    public static float var(float[] a) {
        float avg = mean(a);
        float sum = 0f;
        foreach (float f in a){
            sum += (float) Math.Pow((double) (f - avg),(double)2);
        }
        return sum / (a.Length - 1);
    }

    /**
    * Return sample standard deviation of array, NaN if no such value.
    */
    public static float stddev(float[] a) {
        return (float) Math.Sqrt(var(a));
    }

    /**
    * Return sum of all values in array.
    */
    public static float sum(float[] a) {
        float sum = 0f;
        foreach (float f in a){
            sum += f;
        }

        return sum;
    }

    /**
    * Return sum of all values in array.
    */
    public static double sum(double[] a) {
        double sum = 0f;
        foreach (double f in a) {
            sum += f;
        }

        return sum;
    }
    /*
     * y = ax + b where result[0]=a and result[1] = b
     * and correlation r = result[2]
     */
    public static float[] resampleLinearRegression(float[] x, float[] y){
        float[] result = new float[]{0f,0f,0f};
        int n = x.Length;
        float sumX = 0, sumY = 0,sumXY = 0, sumX2 = 0, sumY2 = 0;
        sumX = sum(x);
        sumY = sum(y);
        for(int i = 0; i < n; i++){
            sumXY += x[i]*y[i];
            sumX2 += (float)Math.Pow(x[i], 2);
            sumY2 += (float)Math.Pow(y[i], 2);
        }



        try{
            result[0] = (n*sumXY-sumX*sumY)/(n*sumX2-(float)Math.Pow(sumX, 2));
            //result[1] = (sumY-result[0]*sumX)/n;
            //result[2] = ( n*sumXY-sumX*sumY )/( (float)Math.sqrt( (n*sumX2-(float)Math.pow(sumX, 2))*(n*sumY2-Math.pow(sumY,2)) ) );
        }catch(Exception ){
            return new float[]{0f,0f,0f};
        }
        return result;
    }

    private static void solveTridiag(float[] sub, float[] diag , float[] sup , float[] b , int n){
        /*                  solve linear system with tridiagonal n by n matrix a
                    using Gaussian elimination *without* pivoting
                    where   a(i,i-1) = sub[i]  for 2<=i<=n
                            a(i,i)   = diag[i] for 1<=i<=n
                            a(i,i+1) = sup[i]  for 1<=i<=n-1
                    (the values sub[1], sup[n] are ignored)
                    right hand side vector b[1:n] is overwritten with solution
                    NOTE: 1...n is used in all arrays, 0 is unused */
        int i;
        /*                  factorization and forward substitution */
        for(i=2; i<=n; i++){
            sub[i] = sub[i]/diag[i-1];
            diag[i] = diag[i] - sub[i]*sup[i-1];
            b[i] = b[i] - sub[i]*b[i-1];
        }
        b[n] = b[n]/diag[n];
        for(i=n-1;i>=1;i--){
            b[i] = (b[i] - sup[i]*b[i+1])/diag[i];
        }
    }
 
    public static float[,] resampleBicubic(float[] x, float[] d, int precision ) {
            int np = x.Length;    
            float[,] interpol = new float[2,0];
            
            float y;
            float t;
            float[] a = new float[np];
            float t1;
            float t2;
            float[] h = new float[np];
            for (int i=1; i<=np-1; i++){
                h[i] = x[i] - x[i-1];
            }

            float[] sub = new float[np-1];
            float[] diag = new float[np-1];
            float[] sup = new float[np-1];

            for (int i=1; i<=np-2; i++){
                diag[i] = (h[i] + h[i+1])/3;
                sup[i] = h[i+1]/6;
                sub[i] = h[i]/6;
                a[i] = (d[i+1]-d[i])/h[i+1]-(d[i]-d[i-1])/h[i];
            }
            solveTridiag(sub,diag,sup,a,np-2);
            int nprecision = (np-1)*precision;
            interpol = new float[nprecision, nprecision ];
            int count = 0;
            for (int i=1; i<=np-1; i++) {   // loop over intervals between nodes
                for (int j=1; j<=precision; j++){
                    t1 = (h[i]*j)/precision;
                    t2 = h[i] - t1;
                    y = ((-a[i-1]/6*(t2+h[i])*t1+d[i-1])*t2 + (-a[i]/6*(t1+h[i])*t2+d[i])*t1)/h[i];
                    t=x[i-1]+t1;
                    interpol[0,count] = t;
                    interpol[1,count] = y;
                    count++;
                }
            }

            return interpol;
    }
 
    public static float[] resampleWindow(float[] time, float[] data, int window, int precision)
    {
        float[,] dataInterpoled = resampleBicubic(time, data, precision);
        float[] dataResampled = new float[window];
        int inc = (int)Math.Round( (double)( (time.Length - 1) * precision) / window);
        int k = 0;
        for (int i = 0; i < window; i++)
        {
            dataResampled[k] = dataInterpoled[1,i * inc];
            k++;
        }
        return dataResampled;

         
    }

    public static double[] removeConsecutivesEquals(double[] data){

        double val = data[0];
        int valid = 1;
        for(int i = 1; i <data.Length;i++){
            if (data[i] == val){
                data[i] = 0;
            } else{
                val = data[i];
                valid++;
            }
        }
        double[] temp = new double[valid];
        int count = 0;
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] != 0){
                temp[count] = data[i];
                count++;
            }else{
               // System.out.println(data[i]);
            }
        }
        return temp;

    }

    /* This function detrends (subtracts a least-squares fitted line from) a
   a sequence of n uniformily spaced ordinates supplied in c. */
    public static void detrend(float[] c, int n) {
        int i;
        float a, b = 0f, tsqsum = 0f, ysum = 0f, t;

        for (i = 0; i < n; i++)
            ysum += c[i];
        for (i = 0; i < n; i++) {
            t = (i - n / 2 + 0.5f);
            tsqsum += t*t;
            b += t*c[i];
        }
        b /= tsqsum;
        a = (ysum / n - b * (n - 1) / 2.0f);
        for (i = 0; i < n; i++)
            c[i] -= a + b*i;
        if (b < -0.04 || b > 0.04){}
            //System.out.print("(warning) possibly significant trend in input series\n");
    }

     /* This function detrends (subtracts a least-squares fitted line from) a
   a sequence of n uniformily spaced ordinates supplied in c. */
    public static void detrend(double[] c, int n) {
        int i;
        double a, b = 0d, tsqsum = 0d, ysum = 0d, t;

        for (i = 0; i < n; i++)
            ysum += c[i];
        for (i = 0; i < n; i++) {
            t = (i - n / 2 + 0.5d);
            tsqsum += t*t;
            b += t*c[i];
        }
        b /= tsqsum;
        a = (ysum / n - b * (n - 1) / 2.0d);
        for (i = 0; i < n; i++)
            c[i] -= a + b*i;
        if (b < -0.04 || b > 0.04){}
            //System.out.print("(warning) possibly significant trend in input series\n");
    }
    /* This function calculates the mean of all sample values and subtracts it
       from each sample value, so that the mean of the adjusted samples is zero. */

    public static void zeromean(float[] y) {
        int l;
        float vmean = 0.0f;
        vmean = mean(y);
        for (l = 0; l < y.Length; l++)
            y[l] -= vmean;
    }

    public static void zeromean(float[] y, int n) {
        float mean = 0.0F;
        mean = sum(y)/n;
        for (int l = 0; l < n; l++)
            y[l] -= mean;
    }
     /**
      * Return absolut distance in array, -infinity if no such value.
      */
    public static double[] absDistance(double[] a) {
        double[] result = new double[a.Length-1];
        for (int i = 0; i < a.Length - 1; i++)
        {
             result[i] = Math.Abs(a[i]-a[i+1]);
        }
        return result;
    }

     /**
      * Return absolut distance in array, -infinity if no such value.
      */
    public static float[] absDistance(float[] a) {
        float[] result = new float[a.Length - 1];
        for (int i = 0; i < a.Length - 1; i++)
        {
            result[i] = Math.Abs(a[i] - a[i + 1]);
        }
        return result;
    }

    public static int indexMax(float[] a)
    {
        float max = float.NegativeInfinity;
        int index = 0;
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] > max) {
                max = a[i];
                index = i;
            }
        }
        return index;
    }

    /**
     * Return minimum value in array, +infinity if no such value.
     */
    public static int indexMin(float[] a) {
        float min = float.PositiveInfinity;
        int index = 0;
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] < min){
                min = a[i];
                index = i;
            }
        }
        return index;
    }

    }



}
