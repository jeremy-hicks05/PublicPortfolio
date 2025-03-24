# C# Console Chess
---
      Tested By: Kari Seitz               
      Early Version Review: Shaun Lake    
      Late Version Review: Jim Shaver

 Description:                                                 
      Imitates a Chess board, allowing user to input          
      a letter and number combination to select a space       
      on the board, and another space to move that space's    
      piece to.                                               
                                                              
      Allows for takebacks and records the history of         
      moves made.                                             
      Allows for viewing list of moves made for the           
      duration of the game.                                   
                                                              
      Checks for checkmate and stalemate.                     
      Follows all the rules of chess.                         

Process:
- Version 1 -> Built, attempted to implement check and checkmate rules, required rewrite.
- Version 2 -> Build, attempted to implement takebacks, required rewrite
- Version 3 -> Leveraged IPiece and IChessMove interfaces, ChessMove and Piece classes with subclasses Capture, En Passant, Castle, etc. and King, Knight, BlackPawn, etc..  This allowed for easier reversal of certain move types by understanding the type of move they were when they were added to the Stack of previously played moves.
