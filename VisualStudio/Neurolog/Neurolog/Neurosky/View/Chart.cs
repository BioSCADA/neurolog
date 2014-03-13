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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
 

namespace BioSCADA
{
    public partial class Chart : UserControl
    {
         
        public bool ChartPlay = true;
        PointPairList waveList = new PointPairList();
        PointPairList atentionList = new PointPairList();
        GraphPane graphPanel;
         
        public Chart()
        { 
            InitializeComponent();
            InitializeChart();
        }

        private void InitializeChart()
        {
            // Get a reference to the GraphPane instance in the ZedGraphControl
            graphPanel = zg1.GraphPane;

            graphPanel.Fill = new Fill(Color.FromArgb(255, 255, 255), Color.FromArgb(250, 250, 250), 90);
           
            graphPanel.Title.Text = "Gráfico de Meditação e Atenção";
            graphPanel.Title.FontSpec.Size = 18;
            graphPanel.Title.IsVisible = false;
            graphPanel.XAxis.Title.Text = "Tempo";
            graphPanel.XAxis.Title.FontSpec.Size = 23;
            graphPanel.YAxis.Title.Text = "Meditação";
            graphPanel.YAxis.Title.FontSpec.Size = 23;
            graphPanel.Y2Axis.Title.Text = "Atenção";
            graphPanel.Y2Axis.Title.FontSpec.Size = 23;
             
            LineItem myCurve = graphPanel.AddCurve("Meditação", waveList, Color.FromArgb(148, 187, 101), SymbolType.Diamond); 
            myCurve.Symbol.Fill = new Fill(Color.White); 
            myCurve = graphPanel.AddCurve("Atenção", atentionList, Color.FromArgb(40, 79, 115), SymbolType.Circle); 
            myCurve.Symbol.Fill = new Fill(Color.White); 
            myCurve.IsY2Axis = true; 
            graphPanel.XAxis.MajorGrid.IsVisible = true; 
            graphPanel.YAxis.Scale.FontSpec.FontColor = Color.FromArgb(148, 187, 101);
            graphPanel.YAxis.Title.FontSpec.FontColor = Color.FromArgb(148, 187, 101); 
            graphPanel.YAxis.MajorTic.IsOpposite = false;
            graphPanel.YAxis.MinorTic.IsOpposite = false; 
            graphPanel.YAxis.MajorGrid.IsZeroLine = false; 
            graphPanel.YAxis.Scale.Align = AlignP.Inside;  
            graphPanel.Y2Axis.IsVisible = true; 
            graphPanel.Y2Axis.Scale.FontSpec.FontColor = Color.FromArgb(40, 79, 115);
            graphPanel.Y2Axis.Title.FontSpec.FontColor = Color.FromArgb(40, 79, 115); 
            graphPanel.Y2Axis.MajorTic.IsOpposite = false;
            graphPanel.Y2Axis.MinorTic.IsOpposite = false; 
            graphPanel.Y2Axis.MajorGrid.IsVisible = true; 
            graphPanel.Y2Axis.Scale.Align = AlignP.Inside; 
            graphPanel.Chart.Fill = new Fill(Color.FromArgb(255, 255, 255), Color.FromArgb(250, 250, 250), 45.0f); 
            zg1.IsShowHScrollBar = true;
            zg1.IsShowVScrollBar = true;
            zg1.IsAutoScrollRange = true;
            zg1.IsScrollY2 = true; 
            zg1.IsShowPointValues = true;
            zg1.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler); 
            zg1.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler( MyContextMenuBuilder); 
            zg1.ZoomEvent += new ZedGraphControl.ZoomEventHandler(MyZoomEvent); 
            zg1.AxisChange(); 
            zg1.Invalidate();
            zg1.AxisChange(); 
        }
         
        public void updatePanel()
        {
            ChartPlay = Protocol.IsPlay;
            if (ChartPlay)
            { 
                double x = (double)waveList.Count; 
                double y = Protocol.Attention;
                double y2 = Protocol.Meditation;
                if (y+y2 > 0)
                {
                    waveList.Add(x, y);
                    atentionList.Add(x, y2);
                    if (x < 500)
                    {
                        graphPanel.XAxis.Scale.Min = 0;
                        graphPanel.XAxis.Scale.Max = x + 100;
                        zg1.RestoreScale(graphPanel);
                    }
                    else
                    {
                        graphPanel.XAxis.Scale.Min = x - 500;
                        graphPanel.XAxis.Scale.Max = x + 100;

                    } 
                }

                zg1.AxisChange();
                zg1.Invalidate();
                zg1.Refresh();
                AlarmMessageBus.log((System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#94bb65"), "Raw: " + Protocol.Raw + " Meditação:" + Protocol.Meditation + " Atenção: " + Protocol.Attention);
            }
             Refresh();
        }
          
        private string MyPointValueHandler(ZedGraphControl control, GraphPane pane, CurveItem curve, int iPt)
        {
            // Get the PointPair that is under the mouse
            PointPair pt = curve[iPt]; 
            return curve.Label.Text + " é " + pt.Y.ToString("f2") + " no timestamp " + pt.X.ToString("f1") + " ";
        }
         
        private void MyContextMenuBuilder(ZedGraphControl control, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
        {
            
        }
          
        private void MyZoomEvent(ZedGraphControl control, ZoomState oldState, ZoomState newState)
        {
           
        }
  
        private void CharPanel_Resize(object sender, EventArgs e)
        {
            zg1.Size = new Size(pDockChart.Width, pDockChart.Height);
        }

        private void zg1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.ChartPlay)
            {
                
            }
            else
            {
 
            }
        }
  
        internal void Clear()
        {
            waveList.Clear();
            atentionList.Clear();
        }

        private void zg1_Load(object sender, EventArgs e)
        {

        }
    }
}
