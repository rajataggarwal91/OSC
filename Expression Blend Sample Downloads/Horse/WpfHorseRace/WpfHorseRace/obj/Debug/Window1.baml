   M S B A M L   `   `   `  ����N  JWpfHorseRace, Version=1.0.3958.25640, Culture=neutral, PublicKeyToken=null+clr-namespace:WpfHorseRaceWpfHorseRace  5          WpfHorseRace.Window1   5   	   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      *localclr-namespace:WpfHorseRace    5        � Title$  The WPF Derby��5      
�������$	��710��5      $	��710��6    � WindowStartupLocation$ CenterScreen=�5      $��13(�5	      .��5      � ��5      �� 5       �� ImageSource$ Resources/Background.jpg��6    �� Opacity$
 0.25Q�6:    � RowDefinitions 5      �� 5      $��* �6   �� 5      $
��Auto �6    � 	Resources 5       �� MergedDictionaries �� Source   RaceTrackRotateTrans �� Angle�� 5       5   
   �� 5      $* $Resources/RaceHorseDataTemplate.xaml?�6   %%   &	        �� 5   
   $ 0Q�67   5    	   .+�5$      �� -   5        	 ��Name$	 	raceTrack��5$      $��0��5%      $��Stretch=�5&      
 � LayoutTransform#
 [  5'      $��8q�5(      $��Center=�5)      �� 5-      $��1��6   
�������$��1q�6-    �� CornerRadius$ 8��6A   $
��12,4q�6R   ��5/      �� 50   
    �� 
StartPoint$ 0.5,0&�6    �� EndPoint$ 0.5,1&�61   $	 0.8Q�6B   .��51      �  � Color$ #FFFFFF��6    � Offset$ 0Q�6)   � 52      $ #22AA22��6   $	 0.7Q�6)   � 53      $ #448844��6   $
 0.95Q�6)   � 54      $ #EEEEEE��6   $ 1Q�6)   55      ._�57      � .+�58   
   ��  �� Orientation$ 
Horizontal=�6   $��Center=�6.    59       �� 
TargetType �� Property �� Value+   %WpfHorseRace.DoubleToIntegerConverter  RotationDisplayConv��%�   '��       &	 x     �� 5;      " ��6   .�5<      ��  � ��$ Center=�64   �� 5=       � �� �	#EE0000006-   5>        5?      6-   5@      .+�5B      �� $
��10,4q�6   	Rotation:60   �� 5D      -   5         ��Name$ rotationSlider��5D      $	��2,0q�5E       � Minimum$	 -90Q�5F       � Maximum$ 90Q�6   $	��120��5G       � Value 5H      ��  �� Source# [   �� Path$ Angle��� 5J      ��6   ��  �� ElementName$ rotationSlider��$ Value� �� 	Converter# [ �� 5K      2  ,System.Windows.Markup.XmlAttributeProperties  XmlSpace preserve6   
 degrees65   5L      �� 5N   
   $
��10,4q�6   $��Right=�6"   �� 5O      -   5         ��Name$ lnkStartNewRace��5O      .�6/   Start new race6?   5P      5Q   	   5R      5S      5T      