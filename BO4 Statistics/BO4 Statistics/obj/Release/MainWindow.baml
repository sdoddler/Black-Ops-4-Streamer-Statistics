   M S B A M L   `   `   `  ���� Z  VBlack Ops 4 Streamer Statistics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:BO4_StatisticsBO4_Statistics  E'clr-namespace:BO4_Statistics.PropertiesBO4_Statistics.Properties      BO4_Statistics.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     .localclr-namespace:BO4_Statistics     >
Properties'clr-namespace:BO4_Statistics.Properties     -     � Title$  
MainWindow��$	��450��$	��800��.��� .+��� $��
PlayerName��$
��Left=�$��10,8,0,0q�$	��Top=�$	��132��$��27���� -    ��Name$ PlayerName_�� �� IsTextSearchEnabled .  �� 
IsEditable . $
��Left=�$��23��$��	22,35,0,0q� �� Text$ doddler#6282��$	��Top=�$	��120���� -    ��Name$ GetStats��$��Refresh Stat Preview��$
��Left=�$��172,163,0,0q�$	��Top=�$	��111���� -   $ Platform��$
��Left=�$��
163,35,0,0q�$	��Top=�$	��120��.@��� $��
Battle.net�� �� 
IsSelected . �� $	��PSN���� $
��Xbox���� -   $
 Stat��$
��Left=�$��
22,135,0,0q�$	��Top=�$	��261���� -    ��Name$ StatDisplay��$
��Left=�$��
22,164,0,0q�$
��Wrap=�$��Stat Preview��$	��Top=�$��22��$	��154���� -   $ StatType��$
��Left=�$��
22,100,0,0q�$	��Top=�$	��120��.@��� $��MP�� . �� $��Blackout���� $��Zombies���� -   $ StatMode��$
��Left=�$��22��$��166,100,0,0q�$	��Top=�$	��117�� �� SelectedIndex$ 0��.@��� $��
Stat Total���� $��Session Total���� -	   	 ��Name$	 LogBox��$
��Left=�$
��Auto��$#��22,256,0,10                  q�
 �� TextWrapping$

 Wrap=�$��Log Box��$��Stretch=�$	��261��$��10(� �� 
CaretBrush �	#FF020202 �� SelectionOpacity$ 0Q� �� 
IsReadOnly . $ �� HorizontalScrollBarVisibility$
 Auto=�" �� VerticalScrollBarVisibility$
 Auto=��� -
   $ Restart_Session��$��Restart Session��$
��Left=�$��
526,48,0,0q�$	��Top=�$��88��$��20���� -   $ Toggle_Auto_Refresh��$��Toggle Auto Refresh��$
��Left=�$��
332,48,0,0q�$	��Top=�$	��121��$��20���� -   $ RefreshStatus��$
��Left=�$��
459,48,0,0q�$
��Wrap=�$	��OFF��$	��Top=�$��20��$��46���� -   $ AddStat��$��Add Stat��$
��Left=�$��115,227,0,0q�$	��Top=�$��74�� J� RenderTransformOrigin$ 0.5,0.5&��� -   $ Remove_Selected��$��Remove Stat(s)��$
��Left=�$��
688,48,0,0q�$	��Top=�$��90��$ 0.5,0.5&��� $��Platform��$
��Left=�$��	151,8,0,0q�$	��Top=�$
��Auto��$
��Auto��&   System.Windows.Controls.DataGrid  -     Name$ ListOfStats��$
��Auto��$��332,78,10,10q�$��Stretch=�$��Stretch=�   
IsReadOnly . $ �� IsSynchronizedWithCurrentItem$
 TrueY� �� DisplayMemberPath$ 
PlayerName��$ 0.5,0.5&�$
��Auto��$	��435��$	��215���� -   $	 
TextPrefix��$
��Left=�$��23��$��100,193,0,0q�$

 Wrap=�$	��Top=�$	��183���� $��	Game Mode��$
��Left=�$��	10,73,0,0q�$	��Top=�$	��132��$��27���� $��Session or Total��$
��Left=�$��
151,74,0,0q�$	��Top=�$
��Auto��$
��Auto���� $��Text Prefix��$
��Left=�$��
22,190,0,0q�$	��Top=�$��78��$��30��