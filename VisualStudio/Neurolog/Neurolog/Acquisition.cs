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
//M
 <People Name="admin" Id="626" >
    <Timestamp>14:00:00 21/12/2013</Timestamp>
    <Duration>478</Duration>
    <Size>1240</Size>
    <Ambient>xxxxxx</Ambient>
    <TAG>ttttttt</TAG>
  </People>
//M*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurolog.Blueteeth
{
    public class Acquisition
    { 
        public int Id;
        public string Timestamp; 
        public string File;
        public int Size;
        public string Type = "NEUROLOG";
        public Acquisition() { }

        public Acquisition(int id, string timestamp, string file, int size, string type)
        {
            File = file;
            Type = type;
            Size = size;
            Id =id; 
            Timestamp = timestamp; 
        }

        public override string ToString()
        {
            return "<?xml version='1.0' encoding='utf-8'?> <Acquisitions><Acquisition> <Id>" + Id + "</Id> <Timestamp>" + Timestamp + "</Timestamp>  <File>" + File + "</File> <Size>" + Size + "</Size> <Type>" + Type + "</Type></Acquisition></Acquisitions>";
        }
    }

    public class Acquisitions : ICollection
    {
        public string CollectionName;
        private ArrayList empArray = new ArrayList();

        public Acquisition this[int index]
        {
            get { return (Acquisition)empArray[index]; }
        }

        public void CopyTo(Array a, int index)
        {
            empArray.CopyTo(a, index);
        }
        public int Count
        {
            get { return empArray.Count; }
        }
        public object SyncRoot
        {
            get { return this; }
        }
        public bool IsSynchronized
        {
            get { return false; }
        }
        public IEnumerator GetEnumerator()
        {
            return empArray.GetEnumerator();
        }

        public void Add(Acquisition newAcquisition)
        {
            empArray.Add(newAcquisition);
        }
    }

}
