   M S B A M L   `   `   `  ����H  DMahApps.Metro, Version=0.9.0.0, Culture=neutral, PublicKeyToken=nullV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	   C ?Neurolog, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:BioSCADABioSCADA 6�      BioSCADA.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   	   9System&clr-namespace:System;assembly=mscorlib     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     6?   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6|   (localclr-namespace:BioSCADA     6�     � Title$  
Neurolog®��5   	    � WindowStartupLocation$ CenterScreen=�6   (   "MahApps.Metro.Controls.MetroWindow   TitleForeground �#999988$��Roboto)�6Z    � Icon$ Images/favicon.ico��6n      
Foreground#���   6�      	TitleCaps .  $	��480��6�   $	��800��6�   .��5      ]�   
Background#���  6   $��0,0,-8,4q�6;   .+�5   
   o� $	��Top=�6    J� SnapsToDevicePixels . $��2,0,0,0q�6?   #���   6P   $y�	Collapsed=�6z   .@�5
      m� $��MAIS...��6   .@�5      m� $��Sobre o BioSCADA��6   .@�5      m� $��Licença��6   m� 5      $��BugTracking��6   5      m� 5      $��How-Tos��6   .@�5      m� $��Reports��6   m� 5      $��	Bluetooth��6   m� 5      $��	Gráficos��6   5      5      5      �� 5   
   -   5         ��Name$ sb_message_bus��5      $��23��6,   $��Bottom=�68   ��6P   �� *SelectedItem.Tag+ �� ElementName$ cmbAlarm��.@�5      �� .��5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� #��[  6    �� 6J   $� ��6N   6Z   �� 5      -   5        	 ��Name$	 	txt_alarm��5       $��5,0,0,0q�63   #��[  6D   ��6m   �� *SelectedItem.Content+$ cmbAlarm��5      5      5      � 5   
   #���  6   .+�5      �� -   5        
 ��Name$
 mainTabControl��5      $��	0,0,10,60q�61   $��Stretch=�6D   &   System.Windows.Media.TextOptions   TextHintingMode$ Animated=�6g   .@�5!      �� $��0q�6   $��Stretch=�6%   $��Stretch=�6C   $��Stretch=�6f   $%��           DASHBOARD           ��6�   $��40��6�    � Cursor$
 Hand��6�   .��5"      � $��20,20,19,19q�6    � RowDefinitions 5#      �� 5$      $��50 �6,   �� 5%      $��25 �6,   �� 5&      $��25 �6,   �� 5'      $��15 �6,   �� 5(      $��25 �6,   �� 5)      $��25 �6,   �� 5*      $��15 �6,   �� 5+      $��40 �6,   �� 5,      $��40 �6,   �� 5-      $��8* �6,   �� 5.      $	��63* �6,    � ColumnDefinitions 50      �� 51      $
��177* �6/   �� 52      $
��177* �6/   �� 53      $
��177* �6/   .+�55        BioSCADA.UserLogin  -   5          Name$ 	UserLogin��55   :   $��1��6*   $	��186��6M   $	��247��6Z   $��Right=�6f   $��8��6�   $	��Top=�6�   $��2��6�   �� 57      $��16(�6$   $��1��62   #��[  6?   $��	Bem vindo��6h   $��0,1,0,23q�6y   $��2��6�   �� 58      -   5        $	 txt_user��58   $   $��16(�66   $��1��6D   #��[  6Q   $��1��6z     System.Environment   UserName#��Z 6�   $��0,1,0,23q�6�   $��2��6�   �� 59      $��16(�6$   $��2��62   #��[  6?   $	��DAQ��6h   $��0,2,0,12q�6s   $��2��6�   �� 5:      $��16(�6$   $��2��62     
GreenColor#���  6?   $��1��6i   $��0,2,0,12q�6�   $��2��6�   ��6y   �� *SelectedItem.Content+$ cmbState��6�   �� 5;      $��3��6$   $��2��61   $��5,8,3,1q�6E   �� 5<      $��6��6$   $��2��61   $��	5,11,3,38q�6E   $��2��6X   �� 5=      $��Center=�6$   $��16(�6?   $��7��6M   #��[  6Z   $
��Modo��6�   $��0,18,0,3q�6�   $��19��6�   �� 5>      -   5         ��Name$ cmbState��5>   J   $��Center=�6#   $��30��6>   $��16(�6\   $��7��6j   $��1��6w    �� SelectedIndex$ 0��6�   $
��Left=�6�   $��	0,12,0,38q�6�   $��2��6�   $	��137��6  .@�5?      ��   BackgroundSelected
 � Tag# [ 6+   $��Monitoramento��6U   �� 5@      $��Simulação��6+    5A   "   �� 5B   &    �� Color$	 Red��66   5D      5E      �� 5F      $��Center=�6$   $��16(�6?   $��8��6M   #��[  6Z   $��Cor do alarme��6�   $��0,19,0,2q�6�   $y�Hidden=�6�   $��19��6�   �� 5G      -   5        $ cmbAlarm��5G   #   $��16(�65   $��8��6C   $��1��6P   $ 0��6`   $	��178��6r   $
��Left=�6~   $��	0,14,0,12q�6�   $��3��6�   $��2��6�   $y�Hidden=�6�   .@�5H      �� # [ 6+   $
��Blue��6U   �� 5I      $��Orange��6+    5J   "   �� 5K   &   $ 	OrangeRed��66   5M      �� 5N      $	��Red��6+    5O   "   �� 5P   &   $	 Red��66   5R      5S      5T      5U      �� 5V      $%��          Dispositivo          ��6   $
 Hand��6C   $��0q�6Q   $��Stretch=�6\   $��Stretch=�6z   $��Stretch=�6�   .��5W      � .+�5X        BioSCADA.DevicePanel  -   5          Name$ panel_device��5X   ,   $
��Auto��6B   5Y      5Z      �� 5[      $%��             Treino            ��6   $
 Hand��6C   $��0q�6Q   $��Stretch=�6\   $��Stretch=�6z   $��Stretch=�6�   .��5\      � $��0q�6   .+�5]        BioSCADA.ChartPanel  $��0q�6+   $
��Auto��66   $
��Auto��6C   5^      5_      �� 5`      $%��            Reports            ��6   $
 Hand��6C   $��0q�6Q   $��Stretch=�6\   $��Stretch=�6z   $��Stretch=�6�   .��5a      � .+�5b        BioSCADA.ReportPanel  -	   5          Name$ report_panel��5b   ,   $
��Auto��6C   $
��Auto��6Q   $��0q�6^   5c      5d      5f      �� 5h      $
��10,0q�6   $ 
Horizontal=�6'   $��50��6@   $��Bottom=�6L     RedColor#���  6g   .+�5i      �� -
   5         ��Name$ bt_play��5i      $
��Play��6*   $��10,0,0,0q�69   $��42��6a   $��60��6m   $
 Hand��6x   �� 5j      -   5        $ bt_stop��5j      $
��Stop��6*   $��10,0,0,0q�69   $��42��6a   $��60��6m   $
 Hand��6x   �� 5k      -   5        $ bt_up��5k      $��Enviar��6(   $��10,0,0,0q�69   $��60��6K   $��42��6W   $
 Hand��6w   5l      5m      5n      5o      