   M S B A M L   `   `   `  ����Z  VBlack Ops 4 Streamer Statistics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:BO4_StatisticsBO4_Statistics  5   	   E'clr-namespace:BO4_Statistics.PropertiesBO4_Statistics.Properties  5   
       BO4_Statistics.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:BO4_Statistics     5   	   >
Properties'clr-namespace:BO4_Statistics.Properties     5   
   -   5          � Title$  
MainWindow��5	   	   $	��450��6   $	��800��6)   .��5      � .+�5   
   �� $��
PlayerName��6   $
��Left=�6%   $��10,8,0,0q�6@   $	��Top=�6R   $	��132��6j   $��27��6v   �� 5   
   -   5         ��Name$ PlayerName_��5       �� IsTextSearchEnabled .  �� 
IsEditable . $
��Left=�6U   $��23��6p   $��	22,35,0,0q�6|    �� Text$ doddler#6282��6�   $	��Top=�6�   $	��120��6�   6�   �� 5   
   -   5         ��Name$ GetStats��5      $��Refresh Stat Preview��6#   $
��Left=�6B   $��172,163,0,0q�6]   $	��Top=�6r   $	��111��6�   6�   �� 5   
   -   5        $ Platform��5      $
��Left=�6%   $��
163,35,0,0q�6@   $	��Top=�6T   $	��120��6l   .@�5      �� $��
Battle.net��6    �� 
IsSelected . 60   �� 5      $	��PSN��6   �� 5      $
��Xbox��6   5      �� 5   
   -   5        $
 Stat��5      $
��Left=�6!   $��
22,135,0,0q�6<   $	��Top=�6P   $	��261��6h   6t   �� 5   
   -   5         ��Name$ StatDisplay��5      $
��Left=�6)   $��
22,164,0,0q�6D   $
��Wrap=�6X   $��Stat Preview��6l   $	��Top=�6�   $��22��6�   $	��154��6�   �� 5   
   -   5        $ StatType��5      $
��Left=�6%   $��
22,100,0,0q�6@   $	��Top=�6T   $	��120��6l   .@�5      �� $��MP��6    . 6(   �� 5      $��Blackout��6   �� 5      $��Zombies��6   5      �� 5   
   -   5        $ StatMode��5      $
��Left=�6%   $��22��6@   $��166,100,0,0q�6L   $	��Top=�6a   $	��117��6y    �� SelectedIndex$ 0��6�   .@�5      �� $��
Stat Total��6   �� 5      $��Session Total��6   5      �� 5   
   -	   5        	 ��Name$	 LogBox��5      $
��Left=�6"   $
��Auto��6=   $#��22,256,0,10                  q�6K   
 �� TextWrapping$

 Wrap=�5       $��Log Box��6(   $��Stretch=�67   $	��261��6S   $��10(�6_    �� 
CaretBrush �	#FF020202 �� SelectionOpacity$ 0Q�6�    �� 
IsReadOnly . $ �� HorizontalScrollBarVisibility$
 Auto=�6�   " �� VerticalScrollBarVisibility$
 Auto=�6�   �� 5!   
   -
   5        $ Restart_Session��5!      $��Restart Session��6*   $
��Left=�6D   $��
526,48,0,0q�6_   $	��Top=�6s   $��88��6�   $��20��6�   �� 5"   
   -   5        $ Toggle_Auto_Refresh��5"      $��Toggle Auto Refresh��6.   $
��Left=�6L   $��
332,48,0,0q�6g   $	��Top=�6{   $	��121��6�   $��20��6�   6�   �� 5#   
   -   5        $ RefreshStatus��5#      $
��Left=�6+   $��
459,48,0,0q�6F   $
��Wrap=�6Z   $	��OFF��6n   $	��Top=�6y   $��20��6�   $��46��6�   �� 5$   
   -   5        $ AddStat��5$      $��Add Stat��6"   $
��Left=�65   $��115,227,0,0q�6P   $	��Top=�6e   $��74��6}    J� RenderTransformOrigin$ 0.5,0.5&�6�   �� 5%   
   -   5        $ Remove_Selected��5%      $��Remove Stat(s)��6*   $
��Left=�6C   $��
688,48,0,0q�6^   $	��Top=�6r   $��90��6�   $ 0.5,0.5&�6�   6�   �� 5&   
   $��Platform��6   $
��Left=�6#   $��	151,8,0,0q�6>   $	��Top=�6Q   $
��Auto��6i   $
��Auto��6v   &   System.Windows.Controls.DataGrid  5'   
   -   5          Name$ ListOfStats��5'      $
��Auto��5(      $��332,78,10,10q�5)      $��Stretch=�6*   $��Stretch=�6H      
IsReadOnly . $ �� IsSynchronizedWithCurrentItem$
 TrueY�5+       �� DisplayMemberPath$ 
PlayerName��5,      $ 0.5,0.5&�62   $
��Auto��6R   $	��435��6_   $	��215��6n   6~   �� 5-   
   -   5        $	 
TextPrefix��5-      $
��Left=�6&   $��23��6A   $��100,193,0,0q�6M   $

 Wrap=�6b   $	��Top=�6v   $	��183��6�   �� 5.   
   $��	Game Mode��6   $
��Left=�6$   $��	10,73,0,0q�6?   $	��Top=�6R   $	��132��6j   $��27��6v   �� 5/   
   $��Session or Total��6   $
��Left=�6+   $��
151,74,0,0q�6F   $	��Top=�6Z   $
��Auto��6r   $
��Auto��6   �� 50   
   $��Text Prefix��6   $
��Left=�6&   $��
22,190,0,0q�6A   $	��Top=�6U   $��78��6m   $��30��6x   52      53      