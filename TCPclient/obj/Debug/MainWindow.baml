   M S B A M L   `   `   `  ����D  @TCPclient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:TCPclient	TCPclient  5   	       TCPclient.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:TCPclient     5   	     � Title$  
MainWindow��5   	   $	��720��6   $
��1280��6)    � Icon$ .\Ressources\spudo.ico��66   .��5	      � ���#23272a � RowDefinitions 5   
   �� 5      $	��17* �6   �� 5      $��1* �6   �� 5      $��2* �6   .+�5   
   �� .��5      �� -   5        $��
stackpanel��5   �   $��0��6   $��Stretch=�6&   $��Stretch=�6B   ���#2c2f33 �� CanVerticallyScroll . .+�5      �� -   5        $��receiveMessage��5   =   
�������$��15(�6/   $��
10,5,300,5q�6S   ���#41444b$y�	Collapsed=�6|   $
��Wrap=�6�   �� 5      -   5        $��sentMessage��5   =   
�������$��15(�6/   $��
300,5,10,5q�6P   ���#41444b$y�	Collapsed=�6y   $
��Wrap=�6�   5      5      � 5   
   $��1��6    � ColumnDefinitions 5      �� 5      $��5* �6#   �� 5      $��1* �6#   .+�5      �� -   5         ��Name$ sendMessage��5   N   $��0��6   
����������#41444b$��23(�6c   6s   �� 5      -   5        $
��send��5   B   $��1��6   $
��Send��6%   $��25(�64   6]   5       � 5"   
   $��2��6    5#      �� 5$      $��4* �6#   �� 5%      �� 5&      .+�5)      �� -   5        $��ip��5)   �   $��0��6   
����������#41444b$��10, 10, 5, 10q�6N   $��IP��6e   $��35(�6o    � ToolTip$ IP��6}   6�   �� 5*      -   5        $
��port��5*   ~   $��1��6   
����������#41444b$��5, 10, 5, 10q�6N   $
��Port��6d   $��35(�6p   6�   �� 5+      -   5        $��connect��5+   \   $��2��6   $��5, 10, 10, 10q�6%   $��Connect��6<   $��35(�6N   6�   5,      5-      5.      