   M S B A M L   `   `   `  ����N  JWpfHorseRace, Version=1.0.3958.25640, Culture=neutral, PublicKeyToken=null+clr-namespace:WpfHorseRaceWpfHorseRace  5      \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35  �� MergedDictionaries �� Source �� DataType    WpfHorseRace.RaceHorse � 	Resources   Key �� 
StartPoint �� EndPoint � Color � Offset;   5WpfHorseRace.RaceHorseProgressIndicatorWidthConverter ��Name  RacePitBorderStyle	 �� ElementName
 �� Path �� Orientation ��Name  RaceInProgressBrush �� RadiusX �� RadiusY  	WidthConv `� 	Converter ��Name � ToolTip �� Triggers ^� 
Conditions �� Binding �� Value �� 
TargetName  FinishedBrush �� Value �� Property N� EnterActions �� 
TargetName �� TargetProperty	 W� To g� Duration N� ExitActions �� Value  �� Binding  WinnerBrush�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    5      $localclr-namespace:WpfHorseRace  5        5      �� 5	      $ RacePitBorderStyle.xaml?�6   %x	  (��       5      *,  +)7  7  7  7  7  �� 6   "   6    5      ���� 5      RaceInProgressBrush  $ 0,0.5&�68   $ 1,0.5&�6K   .��5   
   � $ 	#1100CC22��6   $ 0Q�6)   � 5   
   $ 	#8800CC22��6   $
 0.97Q�6)   � 5   
   $ 	#AA10FF18��6   $ 0.999Q�6)   � 5   
   $ 	#44FFFFFF��6   $ 1Q�6)   5   	   �� 5      FinishedBrush  $ 0.5,0&�62   $ 0.5,1&�6E   .��5   
   � $ 	#44FF0000��6   $	 0.1Q�6)   � 5   
   $ 	#11FF0000��6   $	 0.9Q�6)   5   	   �� 5      WinnerBrush  $ 0.5,0&�60   $ 0.5,1&�6C   .��5   
   � $
 Gold��6   $	 0.1Q�6$   � 5   
   $ 	#FFFFFF88��6   $	 0.9Q�6)   5   	     5      	WidthConv  67   5       .R�5"      �� $ racePit��6   8��  ._�5#      � ��6   �� $	 racePit��$
 ActualWidth�.+�5$   
   �� $ 
Horizontal=�6   .+�5&      �� $ progressIndicator��6   8�� $
��Left=�5(      $ 8��5)      $ 8��5*      $��1q�5+      ��5/      `� 50      8  .1�51      �� $
 PercentComplete�6   �� 52      $	 racePit��6   $
 ActualWidth�60   53      55      �� 58      $��RaceHorse.gif��6   $��60��6)   $��80��65   59      �� 5;   
   $ 	horseName��6   $	��6,0q�6>   $��Center=�6K   
���������6'   �� *Name+6f   5<   	    5?      _� 5@   
   .��5A      � .+�5B      �� �� 5C      $��0,0,-2,0q�6;   ��6   �� *PercentComplete+6;    �� 5D      .�6   
% finished6    5E      5F      5G      5I       5K      ^� 5M       5N   
   �� 5O       6   �� *
IsFinished+ True65   �� 5P       6   �� *
IsWinner+	 False63   .0�5S   
   �� $ progressIndicator��6   8   � ��6@    5V   
   �� 5W      .��5X      �� .��5Y      W� $ racePit��6    $ Opacity�6@   $	 0.6X�6d   $ 0:0:0.75D�6m   5Z      5[       5]   
   �� 5^      .��5_      �� .��5`      W� $ racePit��6    $ Opacity�6@   $ 1X�6d   $ 0:0:0.25D�6k   5a      5b      5d   	   �� 5g      $
 True��61     6   �� *
IsWinner+.a�5h   
   �� $ progressIndicator��6   8   � ��6@   �� 5i   
   $ 	horseName��6    � ��
 �   �6>   �� 5j   
   $ 	horseName��6    � ��$
 Bold"�6>   �� 5k   
   $ 	horseName��6    � ��$ Center=�6G   5l   	   5n      5o      