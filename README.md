# BookReaderUmo

BoookReaderBru can be used as normal UCI chess engine in chess GUI like Arena.<br/>
To use this program you need install  <a href="https://dotnet.microsoft.com/download/dotnet-framework/net48">.NET Framework 4.8</a>

## Parameters

**-bn** opening Book file Name<br/>
**-ef** chess Engine File name<br/>
**-ea** chess Engine Arguments<br/>
**-lr** Limit maximum ply depth when Read from book (default 0) 0 means no limit<br/>
**-lw** Limit maximum ply depth when Write to book (default 0) 0 means no limit<br/>

### Examples

-bn book.umo -lw 10<br/>
book -lw 10

Opens a chess library of openings called "book.rbu", and if the player gets a checkmate, the first 10 half moves of the game will be added to the library

-bn book.rbu -ef stockfish.exe<br />
book -ef stockfish.exe

Opens a chess library of openings named book.umo, and if it doesn't find any move in it, it will run a chess engine called stockfish.exe 
