   M S B A M L   `   `   `  ����N  JSpeakerVerification, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:SpeakerVerificationSpeakerVerification  5   	   $    SpeakerVerification.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:SpeakerVerification     5   	   -   5          � Title$  
MainWindow��5   	   $	��350��6   $	��525��6)   .��5	      � .+�5
   
   �� -   5         ��Name$ 	recordBtn��5
      $��Record��6$   $
��Left=�65   $��	243,0,0,0q�6P   $	��Top=�6c   $��75��6{    J� RenderTransformOrigin$ -0.976,-1.026&�6�   �� 5   
   -   5        $ stopRecordBtn��5      $
��Stop��6(   $
��Left=�67   $��	323,0,0,0q�6R   $	��Top=�6e   $��75��6}   6�   �� 5   
   -   5         ��Name$ remEnrollLabel��5      $��Remaining Enrollments��6(   $
��Left=�6H   $��
10,279,0,0q�6c   $	��Top=�6w   $ -0.818,4.626&�6�   �� 5   
   -   5        $ verPhraseLabel��5      $��Verification Phrase:��6(   $
��Left=�6G   $��
10,253,0,0q�6b   $	��Top=�6v   $ 0.488,0.898&�6�   �� 5   
   -   5         ��Name$ verPhraseText��5      $
��Left=�6)   $��23��6D   $��166,256,0,0q�6P    �� TextWrapping$
 Wrap=�6e   $��Verification Phrase��6y   $	��Top=�6�   $	��323��6�   �� 5   
   -   5        $ remEnrollText��5      $
��Left=�6)   $��23��6D   $��166,284,0,0q�6P   $
 Wrap=�6e   $��Remaining Enrollments��6y   $	��Top=�6�   $	��120��6�   �� 5   
   -   5        $ button2��5      $��Verify��6"   $
��Left=�63   $��	403,0,0,0q�6N   $	��Top=�6a   $��75��6y   6�   �� 5   
   -	   5         ��Name$ phrasesList��5      $
��Left=�6(   $	��219��6C   $��	10,10,0,0q�6P   $	��Top=�6c   $	��228��6{    �� View 5      �� 5      .P�5      �� 5      5      �� 5   
   -
   5        $ confidenceLbl��5      $��Confidence Level��6'   $
��Left=�6B   $��276,119,0,0q�6]   $	��Top=�6r   �� 5   
   -   5        $ 	statusLbl��5      $��Status��6#   $
��Left=�64   $��
276,37,0,0q�6O   $	��Top=�6c   �� 5   
   -   5        $ confTxt��5      {�.  $
��Left=�66   $��24��6Q   $��276,150,0,0q�6]   $
 Wrap=�6r   $��Confidence Level��6�   $	��Top=�6�   $	��241��6�   �� 5   
   -   5        $ statusResTxt��5      {�.  $
��Left=�6:   $��24��6U   $��
276,63,0,0q�6a   $
 Wrap=�6u   $��Verification Status��6�   $	��Top=�6�   $	��241��6�   �� 5   
   -   5        $ 	enrollBtn��5      $��Enroll��6$   $
��Left=�65   $��
403,27,0,0q�6P   $	��Top=�6d   $��75��6|   6�   5      5      