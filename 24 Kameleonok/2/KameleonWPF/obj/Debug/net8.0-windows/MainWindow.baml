   M S B A M L   `   `   `  ����F  BKameleonWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:KameleonWPFKameleonWPF  5   	       KameleonWPF.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:KameleonWPF     5   	     � Title$  
Kameleonok��5   	   $	��500��6   $	��500��6)   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��5* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $��1* �6   �� 5      $��1* �6   .+�5   
   o� $��0��6   $��2��6   .@�5      m� $
��Menu��6   .@�5      m� $��New Game��6   .@�5      m� $	��3x3��6    m� Command 6,   �� *ThreeGameCommand+m� 5      $	��5x5��6    6,   �� *FiveGameCommand+m� 5      $	��7x7��6    6,   �� *SevenGameCommand+5      �� 5      m� 5      $��Save game...��6    61   �� *SaveGameCommand+{�6U   �� *
EnableSave+m� 5      $��Load game...��6    61   �� *LoadGameCommand+{�6U   �� *
EnableLoad+�� 5      m� 5       $
��Exit��6    6)   �� *ExitCommand+5!      5&      �� 5)   
   $��1��6   $��0��6$   $��2��6Z   $��0,5,5,0q�6n   ��64   �� *ColumnHeaders+��5*      �� 5+      .R�5,      A�  A� Rows$ 1��6"    A� Columns 6+   �� *Size+5-      ��5/      �� 50      .R�51      �� $��40��6a   $��40��6o   $|�False��52       J� RenderTransformOrigin$ 0.5, 0.5&�6#   $��24(�6D   $
��Bold"�6R   $��0,0,0,10q�6d   ��51      �� *StepCommand+��6=   �� *Number+53      54      56      �� 59   
   $��2��6   $��0��6$   $��2��6S   $��0,0,5,0q�6g   ��64   �� *Fields+��5:      �� 5;      .R�5<      A�  6"   �� *Size+ 68   �� *Size+5=      ��5?      �� 5@      .R�5A      �� $
{�True��6a   $
|�True��6r   $ 0.5, 0.5&�6�   $��20(�5B      $
��Bold"�6'   ��5A      �� *StepCommand+��6=   �� *Number+��5B   9   �� *Colour+.��5C      �� ��6    �� *Image+5D      5E      5G      �� 5J   
   $��3��6   $��Center=�6!   $��2��6<   y�6P   �� *StatusBarVisibility+.@�5K      � $��22��6   $��Center=�6   $	��446��6:    5L      �� 5M      $	��150 �6'   �� 5N      $��22 �6'   �� 5O      $	��113 �6'   �� 5P      $��41 �6'   �� 5Q      $	��120 �6'   �� 5R      $��94 �6'   .+�5U      �� $��0��6    $��P1 time:��60   $
��Left=�6C   $��10,0,0,0q�6^   $��53��6p   �� 5V      $��	60,0,44,0q�6;   ��6    �� *P1Time+6;   �� 5W      $��2��6    $��P2 time:��60   $
��Left=�6C   $��10,0,0,0q�6^   $��54��6p   �� 5X      $��2��6    $��59,0,5,0q�6K   ��60   �� *P2Time+6K   5[      5\      5]      5^      