   M S B A M L   `   `   `  ����G  CBekeritesWpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:RobotMalacWpfRobotMalacWpf  5   	       RobotMalacWpf.MainWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:RobotMalacWpf     5   	     � Title$  Robot Pig Fight��5      $	��550��6)   $	��800��66   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��5* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $
��1.5* �6   �� 5      $��1* �6   .+�5   
   o� $��0��6   $��2��6   .@�5      m� $
��Menu��6   .@�5      m� $��New Game��6   .@�5      m� $	��3x3��6    m� Command 6,   �� *FourGameCommand+m� 5      $	��5x5��6    6,   �� *SixGameCommand+m� 5      $	��7x7��6    6,   �� *EightGameCommand+5      �� 5      m� 5      $��Save game...��6    61   �� *SaveGameCommand+m� 5      $��Load game...��6    61   �� *LoadGameCommand+�� 5      �� 5       m� 5!      $
��Exit��6    6)   �� *ExitCommand+5"      5#      �� 5%   
   $��1��6   $��0��6$   ��64   �� *Fields+��5'      �� 5(      .R�5*      A�  A� Rows 6"   �� *Size+ A� Columns 68   �� *Size+5+      ��5-      �� 5/      .R�50      �� $
{�True��6z   $|�False��6�    J� RenderTransformOrigin$ 0.5, 0.5&�6�   $��20(�51      $
��Bold"�6+   ��50      �� *Text+��66   �� *StepCommand+��6V   �� *Number+53      56      ��58      �� 5:      .�5;      ��  �� Property � �� �� Value 61   �� *X+�� 5<       � �� 64   �� *Y+5=      5?      �� 5D   
   $��1��6   $
��Left=�6p   $��
129,20,0,0q�6�   $��1��6�   $	��Top=�6�   ��6    �� *PlayerTurnText+y�6C   �� *RightColumnVisibility+6�   �� 5E   
   $��1��6   $��Remaining steps:��6    $
��Left=�6h   $��	99,49,0,0q�6�   $��1��6�   $	��Top=�6�   y�6;   �� *RightColumnVisibility+6�   �� 5F   
   $��1��6   $
��Left=�6p   $��
210,49,0,0q�6�   $��1��6�   $	��Top=�6�   ��6    �� *RemainingSteps+y�6C   �� *RightColumnVisibility+6�   5P      5Q      