   M S B A M L   `   `   `  ����B  >MaciWpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:MaciWpfMaciWpf  5   	       MaciWpf.MainWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:MaciWpf     5   	     � Title$  	Maci Laci��5      $	��550��6#   $	��800��60    J� InputBindings 5
      �� 5   
   
 �� Key$ W��6    �� CommandParameter$ w��6<    �� Command 6   �� *
KeyCommand+6<   �� 5   
   $ Up��6   $ w��6=    6   �� *
KeyCommand+6=   �� 5   
   $ A��6   $ a��6<    6   �� *
KeyCommand+6<   �� 5   
   $
 Left��6   $ a��6?    6    �� *
KeyCommand+6?   �� 5   
   $ S��6   $ s��6<    6   �� *
KeyCommand+6<   �� 5   
   $
 Down��6   $ s��6?    6    �� *
KeyCommand+6?   �� 5   
   $ D��6   $ d��6<    6   �� *
KeyCommand+6<   �� 5   
   $ Right��6   $ d��6@    6!   �� *
KeyCommand+6@   �� 5   
   $ B��6   $ b��6<    6   �� *
KeyCommand+6<   �� 5   
   $	 Esc��6    6   �� *PauseStartGameCommand+.��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��5* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $
��1.5* �6   �� 5      $��1* �6   .+�5"   
   o� $��0��6   $��2��6   .@�5#      m� $
��Menu��6   .@�5$      m� $��New Game��6   .@�5%      m� $	��4x4��6    m� Command 6,   �� *FourGameCommand+m� 5&      $	��6x6��6    6,   �� *SixGameCommand+m� 5'      $	��8x8��6    6,   �� *EightGameCommand+5(      �� 5)      �� 5,      m� 5-      $
��Exit��6    6)   �� *ExitCommand+5.      m� 5/      $��Pause��6    6&   �� *PauseStartGameCommand+51      52      �� 54   
   $��1��6   $��0��6$   ��64   �� *Fields+��56      �� 57      .R�59      A�  A� Rows 6"   �� *Size+	 A� Columns	 68   �� *Size+5:      ��5<      �� 5>      .R�5?      �� $
{�True��6   $|�False��6/   
 J� RenderTransformOrigin$
 0.5, 0.5&�6A   $��20(�5@      $
��Bold"�6+   
������ ��6V   �� *Colour+5B      5E      ��5G      �� 5I      .�5J      ��  �� Property � �� �� Value 61   �� *X+�� 5K       � �� 64   �� *Y+5L      5N      �� 5P   
   $��1��6   $
��Left=�6r   $��
130,82,0,0q�6�   $��1��6�   $	��Top=�6�   ��6    �� *PicnicBasketsAcq+y�6E   �� *RightColumnVisibility+6�   �� 5R   
   $��1��6   $��Picnic baskets:��6    $
��Left=�6g   $��	19,52,0,0q�6�   $��1��6�   $	��Top=�6�   y�6:   �� *RightColumnVisibility+6�   �� 5S   
   $��1��6   $
��Left=�6o   $��
130,52,0,0q�6�   $��1��6�   $	��Top=�6�   ��6    �� *PicnicBaskets+y�6B   �� *RightColumnVisibility+6�   �� 5U   
   $��1��6   $��Elapsed time:��6    $
��Left=�6e   $��	19,20,0,0q�6�   $��1��6�   $	��Top=�6�   y�68   �� *RightColumnVisibility+6�   �� 5V   
   $��1��6   $
��Left=�6l   $��
100,20,0,0q�6�   $��1��6�   $	��Top=�6�   ��6    �� *
PlayerTime+y�6?   �� *RightColumnVisibility+6�   5W      5X      