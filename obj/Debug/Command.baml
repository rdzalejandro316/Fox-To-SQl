   M S B A M L   `   `   `  ����C  ?FoxToSql, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:FoxToSqlFoxToSql  5   	       FoxToSql.Command   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   b ^MaterialDesignThemes.Wpf, Version=3.2.0.1979, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   "localclr-namespace:FoxToSql  5   	   -   5          � Title$  Command��5   	   $	��300��6   $	��500��6&    � 	Resources 5
       �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader �� Property �� Value �� MergedDictionaries �� Source�� 5   
    5      �� 5      $e _pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml?�6%   �� 5      $h bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml?�6%   �� 5      $s mpack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Red.xaml?�6%   %�   '��       '       �� 5      " ��6   �� 5      "  6   .�5      ��  � ��$ 11(�6-   �� 5       � ��$
 Bold"�6/   5      5      .��5      �  � RowDefinitions 5   
   �� 5      �� 5      $��50 �6   .+�5   
   �� $��SQL SERVER COMMAND��6   .��5       �� -   5        $��TxQuery��5        (  $MaterialDesignFilledTextFieldTextBox#��[  6%    �� AcceptsReturn . 	 �� TextWrapping$
	 Wrap=�6x   "
 �� VerticalScrollBarVisibility$

 Auto=�6�   $��5q�6�   5!      �� 5#   
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��Center=�6;   $��Center=�6V   .+�5$      �� -   5        $��
BtnExecute��5$      $��Execute��6'   $	��100��69   $��5q�6E   6P   �� 5%      -   5        $��BtnExit��5%      $
��Exit��6$   $	��100��63   $��5q�6?   6J   5&      5'      5)      