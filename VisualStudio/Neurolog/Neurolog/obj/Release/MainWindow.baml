   M S B A M L   `   `   `  ���� H  DMahApps.Metro, Version=0.9.0.0, Culture=neutral, PublicKeyToken=nullV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem C ?Neurolog, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:BioSCADABioSCADA    BioSCADA.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     9System&clr-namespace:System;assembly=mscorlib     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     (localclr-namespace:BioSCADA       � Title$  
Neurolog®�� � WindowStartupLocation$ CenterScreen=�(   "MahApps.Metro.Controls.MetroWindow   TitleForeground �#999988$��Roboto)� � Icon$ Images/favicon.ico��   
Foreground#���      	TitleCaps .  $	��480��$	��800��.��]�   
Background#���  $��0,0,-8,4q�.+�o� $	��Top=� J� SnapsToDevicePixels . $��2,0,0,0q�#���   $y�	Collapsed=�.@�m� $��MAIS...��.@�m� $��Sobre o BioSCADA��.@�m� $��Licença��m� $��BugTracking��m� $��How-Tos��.@�m� $��Reports��m� $��	Bluetooth��m� $��	Gráficos���� -    ��Name$ sb_message_bus��$��23��$��Bottom=����� *SelectedItem.Tag+ �� ElementName$ cmbAlarm��.@��� .����  �� Orientation$ 
Horizontal=�.+��� #��[  �� $� ���� -   	 ��Name$	 	txt_alarm��$��5,0,0,0q�#��[  ���� *SelectedItem.Content+$ cmbAlarm��� #���  .+��� -   
 ��Name$
 mainTabControl��$��	0,0,10,60q�$��Stretch=�&   System.Windows.Media.TextOptions   TextHintingMode$ Animated=�.@��� $��0q�$��Stretch=�$��Stretch=�$��Stretch=�$%��           DASHBOARD           ��$��40�� � Cursor$
 Hand��.��� $��20,20,19,19q� � RowDefinitions �� $��50 ��� $��25 ��� $��25 ��� $��15 ��� $��25 ��� $��25 ��� $��15 ��� $��40 ��� $��40 ��� $��8* ��� $	��63* � � ColumnDefinitions �� $
��177* ��� $
��177* ��� $
��177* �.+�  BioSCADA.UserLogin  -     Name$ 	UserLogin��$��1��$	��186��$	��247��$��Right=�$��8��$	��Top=�$��2���� $��16(�$��1��#��[  $��	Bem vindo��$��0,1,0,23q�$��2���� -   $	 txt_user��$��16(�$��1��#��[  $��1��  System.Environment   UserName#��Z $��0,1,0,23q�$��2���� $��16(�$��2��#��[  $	��DAQ��$��0,2,0,12q�$��2���� $��16(�$��2��  
GreenColor#���  $��1��$��0,2,0,12q�$��2������ *SelectedItem.Content+$ cmbState���� $��3��$��2��$��5,8,3,1q��� $��6��$��2��$��	5,11,3,38q�$��2���� $��Center=�$��16(�$��7��#��[  $
��Modo��$��0,18,0,3q�$��19���� -    ��Name$ cmbState��$��Center=�$��30��$��16(�$��7��$��1�� �� SelectedIndex$ 0��$
��Left=�$��	0,12,0,38q�$��2��$	��137��.@���   BackgroundSelected
 � Tag# [ $��Monitoramento���� $��Simulação�� ��  �� Color$	 Red���� $��Center=�$��16(�$��8��#��[  $��Cor do alarme��$��0,19,0,2q�$y�Hidden=�$��19���� -   $ cmbAlarm��$��16(�$��8��$��1��$ 0��$	��178��$
��Left=�$��	0,14,0,12q�$��3��$��2��$y�Hidden=�.@��� # [ $
��Blue���� $��Orange�� �� $ 	OrangeRed���� $	��Red�� �� $	 Red���� $%��          Dispositivo          ��$
 Hand��$��0q�$��Stretch=�$��Stretch=�$��Stretch=�.��� .+�  BioSCADA.DevicePanel  -     Name$ panel_device��$
��Auto���� $%��             Treino            ��$
 Hand��$��0q�$��Stretch=�$��Stretch=�$��Stretch=�.��� $��0q�.+�  BioSCADA.ChartPanel  $��0q�$
��Auto��$
��Auto���� $%��            Reports            ��$
 Hand��$��0q�$��Stretch=�$��Stretch=�$��Stretch=�.��� .+�  BioSCADA.ReportPanel  -	     Name$ report_panel��$
��Auto��$
��Auto��$��0q��� $
��10,0q�$ 
Horizontal=�$��50��$��Bottom=�  RedColor#���  .+��� -
    ��Name$ bt_play��$
��Play��$��10,0,0,0q�$��42��$��60��$
 Hand���� -   $ bt_stop��$
��Stop��$��10,0,0,0q�$��42��$��60��$
 Hand���� -   $ bt_up��$��Enviar��$��10,0,0,0q�$��60��$��42��$
 Hand��