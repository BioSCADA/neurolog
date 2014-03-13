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
using BioSCADA;
using Neurolog.Blueteeth;
using MathLibrary;
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZedGraph;

namespace Neurolog
{
    class PhyFFT
    {

        static int N = 16384;	/* maximum points in FFT */

        int m, n, i;
        int decimation = 1;
        int samples = N;
        int nflag;
        int Nflag = 1;

        Boolean plotPower = false;
        int smooth = 1;
        Boolean detrend = false;
        Boolean zeromean = false;
        float[] c;
        float freq, fstep, norm;
        String window;
        float[] input;
        private float[] frequency;
        private float[] power;
        private float[] phase;

        public void TestClass()
        {
            float[] sintet = { 1.0001F, 1.0003F, 0.99646F, 1.0005F, 1.0046F, 0.99293F, 1.0128F, 1.0011F, 0.98939F, 1.0093F, 0.99756F, 0.98586F, 1.0057F, 1.0256F, 0.98232F, 1.0022F, 1.022F, 0.97879F, 0.99865F, 1.0185F, 0.97525F, 0.99511F, 1.015F, 0.97172F, 0.99158F, 1.0114F, 1.0313F, 1.0511F, 0.94479F, 0.96465F, 0.98451F, 1.0044F, 1.0242F, 1.0441F, 1.0639F, 0.95758F, 0.97744F, 0.99729F, 1.0171F, 1.037F, 1.0569F, 0.95051F, 0.97037F, 0.99022F, 1.0101F, 1.0299F, 1.0498F, 0.94344F, 0.9633F, 0.98315F, 1.003F, 1.0229F, 1.0427F, 1.0626F, 1.0824F, 1.1023F, 1.1221F, 0.88959F, 0.90944F, 0.9293F, 0.94916F, 0.96901F, 0.98887F, 1.0087F, 1.0286F, 1.0484F, 1.0683F, 1.0881F, 1.108F, 1.1279F, 0.8953F, 0.91516F, 0.93502F, 0.95487F, 0.97473F, 0.99458F, 1.0144F, 1.0343F, 1.0542F, 1.074F, 1.0939F, 1.1137F, 1.1336F, 0.90102F, 0.92087F, 0.94073F, 0.96059F, 0.98044F, 1.0003F, 1.0202F, 1.04F, 1.0599F, 1.0797F, 1.0996F, 1.1194F, 0.88688F, 0.90673F, 0.92659F, 0.94645F, 0.9663F, 0.98616F, 1.006F, 1.0259F, 1.0457F, 0.81317F, 1.0854F, 0.85288F, 1.1251F, 0.89259F, 1.1649F, 0.93231F, 1.2046F, 0.97202F, 1.2443F, 1.0117F, 0.77918F, 1.0514F, 0.81889F, 1.0912F, 0.8586F, 1.1309F, 0.89831F, 1.1706F, 0.93802F, 1.2103F, 0.97773F, 1.25F, 1.0174F, 0.78489F, 1.0572F, 0.8246F, 1.0969F, 0.86431F, 1.1366F, 0.90403F, 1.1763F, 0.94374F, 1.216F, 0.98345F, 1.2557F, 1.0232F, 0.79061F, 1.0629F, 0.83032F, 1.1026F, 0.87003F, 1.1423F, 0.90974F, 1.182F, 0.94945F, 1.2217F, 0.98917F, 1.2614F, 1.0289F, 0.79632F, 1.0686F, 0.83603F, 1.1083F, 0.87575F, 1.148F, 0.91546F, 1.1877F, 0.95517F, 1.2274F, 0.99488F, 1.2671F, 1.0346F, 0.80204F, 1.0743F, 0.84175F, 1.114F, 0.88146F, 1.1537F, 0.92117F, 1.1934F, 0.96089F, 1.2332F, 1.0006F, 1.2729F, 1.0403F, 0.80775F, 1.08F, 0.84747F, 1.1197F, 0.88718F, 1.1594F, 0.92689F, 1.1992F, 0.9666F, 1.2389F, 1.0063F, 0.77376F, 1.046F, 0.81347F, 1.0857F, 0.85318F, 1.1254F, 0.89289F, 1.1652F, 0.9326F, 1.2049F, 0.97232F, 1.2446F, 1.012F, 0.77947F, 1.0517F, 0.81919F, 1.0915F, 0.8589F, 0.62634F, 1.4034F, 1.1709F, 0.93832F, 0.70577F, 1.4829F, 1.2503F, 1.0177F, 0.78519F, 0.55263F, 1.3297F, 1.0972F, 0.86461F, 0.63206F, 1.4091F, 1.1766F, 0.94404F, 0.71148F, 1.4886F, 1.256F, 1.0235F, 0.79091F, 0.55835F, 1.3354F, 1.1029F, 0.87033F, 0.63777F, 1.4149F, 1.1823F, 0.94975F, 0.7172F, 1.4943F, 1.2617F, 1.0292F, 0.79662F, 0.56407F, 1.3412F, 1.1086F, 0.87604F, 0.64349F, 1.4206F, 1.188F, 0.95547F, 0.72291F, 1.5F, 1.2674F, 1.0349F, 0.80234F, 0.56978F, 1.3469F, 1.1143F, 0.88176F, 0.64921F, 1.4263F, 1.1937F, 0.96118F, 0.72863F, 1.5057F, 1.2732F, 1.0406F, 0.80805F, 0.5755F, 1.3526F, 1.12F, 0.88748F, 0.65492F, 1.432F, 1.1995F, 0.9669F, 0.73434F, 1.5114F, 1.2789F, 1.0463F, 0.81377F, 0.58121F, 1.3583F, 1.1257F, 0.89319F, 0.66064F, 1.4377F, 1.2052F, 0.97261F, 0.74006F, 1.5171F, 1.2846F, 1.052F, 0.81948F, 0.58693F, 1.364F, 1.1315F, 0.89891F, 0.66635F, 1.4434F, 1.2109F, 0.97833F, 0.74578F, 1.5229F, 1.2903F, 1.0578F, 0.8252F, 0.59264F, 1.3697F, 1.1372F, 0.90462F, 0.67207F, 1.4492F, 1.2166F, 0.98405F, 0.75149F, 1.5286F, 1.296F, 1.0635F, 0.83092F, 0.59836F, 1.3754F, 1.1429F, 0.91034F, 0.67778F, 1.4549F, 1.2223F, 0.98976F, 0.75721F, 1.5343F, 1.3017F, 1.0692F, 0.83663F, 0.60408F, 1.3812F, 1.1486F, 0.91605F, 0.6835F, 1.4606F, 1.228F, 0.99548F, 0.76292F, 1.54F, 1.3075F, 1.0749F, 0.84235F, 0.60979F, 1.3869F, 1.1543F, 0.92177F, 0.68922F, 1.4663F, 1.2337F, 1.0012F, 0.76864F, 1.5457F, 1.3132F, 1.0806F, 0.84806F, 0.61551F, 1.3926F, 1.16F, 0.92749F, 0.69493F, 1.472F, 1.2395F, 1.0069F, 0.77435F, 1.5514F, 1.3189F, 1.0863F, 0.85378F, 0.62122F, 1.3983F, 1.1658F, 0.9332F, 0.70065F, 1.4777F, 1.2452F, 1.0126F, 0.78007F, 0.54752F, 1.3246F, 1.092F, 0.85949F, 0.62694F, 1.404F, 1.1715F, 0.93892F, 0.70636F, 1.4834F, 1.2509F, 1.0183F, 0.78579F, 0.55323F, 1.3303F, 1.0978F, 0.86521F, 0.63266F, 1.4097F, 1.1772F, 0.94463F, 0.71208F, 1.4892F, 1.2566F, 1.0241F, 0.7915F, 0.55895F, 1.336F, 1.1035F, 0.87093F, 0.63837F, 1.4155F, 1.1829F, 0.95035F, 0.71779F, 1.4949F, 0.25269F, 1.0298F, 1.8069F, 0.56466F, 1.3417F, 0.099554F, 0.87664F, 1.6537F, 0.41153F, 1.1886F, 1.9657F, 0.72351F, 1.5006F, 0.2584F, 1.0355F, 1.8126F, 0.57038F, 1.3475F, 0.10527F, 0.88236F, 1.6594F, 0.41725F, 1.1943F, 1.9714F, 0.72923F, 1.5063F, 0.26412F, 1.0412F, 1.8183F, 0.57609F, 1.3532F, 0.11099F, 0.88807F, 1.6652F, 0.42296F, 1.2001F, 1.9771F, 0.73494F, 1.512F, 0.26983F, 1.0469F, 1.824F, 0.58181F, 1.3589F, 0.1167F, 0.89379F, 1.6709F, 0.42868F, 1.2058F, 1.9829F, 0.74066F, 1.5177F, 0.27555F, 1.0526F, 1.8297F, 0.58753F, 1.3646F, 0.12242F, 0.8995F, 1.6766F, 0.43439F, 1.2115F, 1.9886F, 0.74637F, 1.5235F, 0.28126F, 1.0584F, 1.8354F, 0.59324F, 1.3703F, 0.12813F, 0.90522F, 1.6823F, 0.44011F, 1.2172F, 1.9943F, 0.75209F, 1.5292F, 0.28698F, 1.0641F, 1.8412F, 0.59896F, 1.376F, 0.13385F, 0.91094F, 1.688F, 0.44583F, 1.2229F, 2F, 0.7578F, 1.5349F, 0.2927F, 1.0698F };
            //float[] rrResampled = Statistics.resampleWindow(null, rr, 256, 4);
            //./fft -f 1 -P -w Welch -Z - ecg01.tach.txt
            long init_time = DateTime.Now.Ticks;


            /*(K-1)D+L = N*/
            new PhyFFT(sintet, 1/*freq*/, 256/*samples*/, 0/*K*/, 256/*L*/, 128/*D*/, false, 1/*smooth*/, 1/*decimation*/, "Welch", true, true);

            Console.WriteLine("total time: " + (DateTime.Now.Ticks - init_time) / 10000);
        }
        public PhyFFT()
        {

        }

        public PhyFFT(float[] input, float freq, int samples, int K, int L, int D, Boolean plotPower, int smooth, int decimation, String win, Boolean zeromean, Boolean detrend)
        {

            
            float[] resamples = new float[samples];
            Array.Clear(resamples, 0, resamples.Length);
            
            if (input.Length < samples)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    resamples[i] = input[i];
                }
            }
            else
            {
                for (int i = (input.Length - samples); i < input.Length; i++)
                {
                    resamples[i - (input.Length - samples)] = input[i];
                }
            }
           

            this.input = resamples;
            this.freq = freq;
            this.samples = samples;
            this.nflag = K;
            this.plotPower = plotPower;
            this.smooth = smooth;
            this.decimation = decimation;
            this.zeromean = zeromean;
            this.detrend = detrend;
            this.window = win;
            decimation = smooth;

           
            Input();

            if (nflag == 0)
            {		/* calculate and print inverse FFT */
                fft();
                output();
            }
            else
            {			/* calculate and print forward FFT */
                float[] s = new float[c.Length];
                for (int k = 0; k < K; k++)
                {
                    for (int z = 0; z < c.Length; z += 2)
                    {
                        c[z] = 0;//initialize c
                        c[z + 1] = 0;//initialize c
                    }
                    //this.input = new float[input.length];
                    // System.out.println(" u : " +  u);
                    for (int j = 0; j < L; j++)
                    {//2/*K*/, 256/*L*/, 128/*D*/
                        if (k == 0)
                        {
                            c[j] = input[(j)];
                            // System.out.println("j: " + j + "\t u: "+ c[j]);
                        }
                        else if (k == 1)
                        {
                            c[j] = input[(j + D)];
                            // System.out.println("j: " + (j + (K-1)*D) + "\t u: "+ c[j]);
                        }
                        else
                        {
                            c[j] = input[(j + (K - 1) * D)];
                            // System.out.println("j: " + (j + (K-1)*D) + "\t u: "+ c[j]);
                        }

                    }
                    if (Nflag == 0)
                    {
                        fft();
                        output();
                    }
                    if (plotPower)
                    {
                        for (int j = 0; j < L; j++)
                        {
                            s[j] += c[j] * c[j];
                        }
                    }
                    else
                    {
                        for (int j = 0; j < L; j++)
                        {
                            s[j] += c[j];
                        }
                    }
                    // System.out.println();
                }

                if (Nflag > 0)
                {
                    for (int z = 0; z < c.Length; z++)
                    {
                        c[z] = 0;//initialize c
                    }
                    if (plotPower)
                    {
                        for (int j = 0; j < L; j++)
                        {
                            if (s[j] != 0)
                            {
                                c[j] = (float)Math.Sqrt(s[j]) / nflag;
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < L; j++)
                        {
                            if (s[j] != 0)
                            {
                                c[j] = s[j] / nflag;
                            }
                        }
                    }
                    fft();
                    output();
                }



            }
        }

        void Input()
        {
            /* Make sure that samples is a power of two. */
            m = N;
            if (this.samples <= N)
            {
                if (this.samples == 0)
                    this.samples = input.Length;

                while (m >= this.samples)
                {
                    m >>= 1;
                }
                m <<= 1;
            }
            else
            {
                while (m < this.samples)
                {
                    m <<= 1;
                }
            }
            // System.out.println( "m \t" + m);
            this.samples = m;
            c = new float[m + 2];
            for (n = 0; n < c.Length; n++)
            {
                c[n] = 0;//initialize c
            }
            for (n = 0; n < input.Length && n < samples; n++)
            {
                try
                {
                    c[n] = input[n];
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        void fft()
        {		/* calculate forward FFT */


            if (zeromean)
            {		/* zero-mean the input array */
                Statistics.zeromean(c, n);
            }
            if (detrend)
            {
                Statistics.detrend(c, n);
            }
            // System.out.println( "samples \t" + samples);
            for (m = samples; m >= n; m >>= 1) { }
            m <<= 1;		/* m is now the smallest power of 2 >= n; this is the
                               length of the input series (including padding) */
            // System.out.println( "m \t" + m);
            float wsum = 0;
            if (!window.Equals(""))
            {			/* apply the chosen windowing function */
                wsum = Windows.apply( c, m, window);
            }
            else
            {
                wsum = m;
            }

            //System.out.println("wsum: "+ wsum);
            norm = (float)Math.Sqrt(2.0 / (wsum * n));
            //System.out.println("norm: "+ norm);

            fstep = (freq / (2.0F * m));


            // System.out.println("fstep: "+ fstep);

            float[] pack = new float[c.Length];
            pack[0] = 0;
            for (int p = 1; p < c.Length - 1; p++)
            {
                pack[p] = c[p - 1];
            }

            FrequencyDomain.realft(pack, m / 2, 1);	/* perform the FFT;  see Numerical Recipes */

            for (int p = 0; p < pack.Length - 1; p++)
            {
                c[p] = pack[p + 1];
            }
        }

        void output()
        {/* print the FFT */


            c[m] = c[1];		/* unpack the output array */
            c[1] = c[m + 1] = 0f;

            frequency = new float[c.Length];
            power = new float[c.Length];

            phase = new float[c.Length];
            Protocol.fftPowerPairList.Clear();
            for (i = 0; i <= m; i += 2 * decimation)
            {
                int j;
                float pow;

                //if (fflag==1)
 ///////////////////////////              /// Console.Write(i * fstep + "\t");
                frequency[i] = i * fstep;

                //	if (cflag==1){
                //            try{
                //                //System.out.println( c[i]+" \t"+c[i+1]);
                //            }catch(Exception ex){
                //                System.out.println(ex.getMessage());
                //            }
                //        }
                //	else {
                for (j = 0, pow = 0.0F; j < 2 * smooth; j += 2)
                {

                    try
                    {
                        pow += (c[i + j] * c[i + j] + c[i + j + 1] * c[i + j + 1]) * norm * norm;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                pow /= smooth / decimation;
               
                if (plotPower)
                {
 ///////////////////////                   Console.WriteLine(pow / (freq / 2));
                    power[i] = pow / (freq / 2);
                }
                else
                {
 ///////////////////////                      Console.WriteLine(Math.Sqrt(pow) / (freq / 2) + "\t");
                   
                    power[i] = (float)Math.Sqrt(pow) / (freq / 2F);
                    Protocol.fftPowerPairList.Add(frequency[i], power[i]);
                }
                //if (false){
                //      try{
                //          //System.out.print( Math.atan2(c[i+1], c[i])+"\t");
                //          phase[i] = Math.Atan2(c[i+1], c[i]);
                //      }catch(Exception ex){
                //          Console.WriteLine(ex.Message);
                //      }

                //  }
                //System.out.println();
                //}
            }
        }

        public float getCoherence()
        {
            float coherence = 0;
            coherence = drawPowerCoerence(getFrequency(), getPower());
            return coherence;
        }

        public float getLF()
        {
            float lf = 0;
            lf = FastStdStats.calculeLF(getFrequency(), getPower());
            return lf;
        }

        public float getHF()
        {
            float hf = 0;
            hf = FastStdStats.calculeHF(getFrequency(), getPower());
            return hf;
        }

        private float drawPowerCoerence(float[] frequency, float[] power)
        {


            float totalPowerMin = 0.04F;
            float totalPowerMax = 0.26F;
            float integratedWindow = 0.015F;
            float coerenceRangeMin = 0.0033F;
            float coerenceRangeMax = 0.4F;
            //int indexXXv = Statistics.indexMax(power);
           // Console.WriteLine(frequency[indexXXv]);
            float[] xx = new float[frequency.Length];
            float[] yy = new float[power.Length];

            Array.Copy(frequency, 0, xx, 0, frequency.Length);
            Array.Copy(power, 0, yy, 0, power.Length);

            for (int i = 0; i < xx.Length; i++)
            {
                if (xx[i] <= totalPowerMin || xx[i] >= totalPowerMax)
                {
                    yy[i] = 0;//zero out off range total power
                }
                 
            }
            float peakPower = 0;
            float totalPower = 0;
            int indexXX = Statistics.indexMax(yy);
            Protocol.PeakFreq = frequency[indexXX];
            
            Array.Copy(power, 0, yy, 0, power.Length);
            for (int i = 0; i < xx.Length; i++)
            {
                if (xx[i] > coerenceRangeMin && xx[i] < coerenceRangeMax)
                {
                    totalPower += yy[i];//zero out off range total power
                }
            }
            
            for (int i = 0; i < xx.Length; i++)
            {
                if (xx[i] > (xx[indexXX] - integratedWindow/2) && xx[i] < (xx[indexXX] + integratedWindow/2))
                {
                    peakPower += yy[i];
                }
            }

            // System.out.println("(totalPower-peakPower)  = " + (totalPower-peakPower) );
            if ((totalPower - peakPower) != 0)
            {
                //  System.out.println("m = " + peakPower/(totalPower-peakPower));
                return (float)Math.Pow(peakPower / (totalPower - peakPower),2);
            }
            else
            {
                //System.out.println(0);
                return 0;
            }
        }

        private float getPeakVLF(float[] x, float[] y, float min, float max)
        {

            float[] xx = new float[x.Length];
            float[] yy = new float[y.Length];
            Array.Copy(x, 0, xx, 0, x.Length);
            Array.Copy(y, 0, yy, 0, y.Length);

            for (int i = 0; i < xx.Length; i++)
            {
                if (!(xx[i] > min && xx[i] < max))
                {
                    yy[i] = 0;
                }
            }
            return Statistics.max(yy);

        }

        private float getPeakLF(float[] x, float[] y, float min, float max)
        {

            float[] xx = new float[x.Length];
            float[] yy = new float[y.Length];
            Array.Copy(x, 0, xx, 0, x.Length);
            Array.Copy(y, 0, yy, 0, y.Length);
            for (int i = 0; i < xx.Length; i++)
            {
                if (!(xx[i] > min && xx[i] < max))
                {
                    yy[i] = 0;
                }
            }
            return Statistics.max(yy);

        }

        private float getPeakHF(float[] x, float[] y, float min, float max)
        {

            float[] xx = new float[x.Length];
            float[] yy = new float[y.Length];
            Array.Copy(x, 0, xx, 0, x.Length);
            Array.Copy(y, 0, yy, 0, y.Length);
            for (int i = 0; i < xx.Length; i++)
            {
                if (!(xx[i] > min && xx[i] < max))
                {
                    yy[i] = 0;
                }
            }
            return Statistics.max(yy);

        }

        /**
         * @return the frequency
         */
        public float[] getFrequency()
        {
            return frequency;
        }

        /**
         * @return the power
         */
        public float[] getPower()
        {
            return power;
        }

        /**
         * @return the phase
         */
        public float[] getPhase()
        {
            return phase;
        }

    }
}
