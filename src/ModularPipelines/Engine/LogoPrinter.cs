using System.Diagnostics.CodeAnalysis;
using Spectre.Console;

namespace ModularPipelines.Engine;

[ExcludeFromCodeCoverage]
internal class LogoPrinter : ILogoPrinter
{
 private const string LargeAsciiLogo = """"""""
                                                                                      
88b           d88                        88               88                          
888b         d888                        88               88                          
88`8b       d8'88                        88               88                          
88 `8b     d8' 88   ,adPPYba,    ,adPPYb,88  88       88  88  ,adPPYYba,  8b,dPPYba,  
88  `8b   d8'  88  a8"     "8a  a8"    `Y88  88       88  88  ""     `Y8  88P'   "Y8  
88   `8b d8'   88  8b       d8  8b       88  88       88  88  ,adPPPPP88  88          
88    `888'    88  "8a,   ,a8"  "8a,   ,d88  "8a,   ,a88  88  88,    ,88  88          
88     `8'     88   `"YbbdP"'    `"8bbdP"Y8   `"YbbdP'Y8  88  `"8bbdP"Y8  88          
                                                                                      
                                                                                      
                                                                                      
88888888ba   88                           88  88                                      
88      "8b  ""                           88  ""                                      
88      ,8P                               88                                          
88aaaaaa8P'  88  8b,dPPYba,    ,adPPYba,  88  88  8b,dPPYba,    ,adPPYba,  ,adPPYba,  
88""""""'    88  88P'    "8a  a8P_____88  88  88  88P'   `"8a  a8P_____88  I8[    ""  
88           88  88       d8  8PP"""""""  88  88  88       88  8PP"""""""   `"Y8ba,   
88           88  88b,   ,a8"  "8b,   ,aa  88  88  88       88  "8b,   ,aa  aa    ]8I  
88           88  88`YbbdP"'    `"Ybbd8"'  88  88  88       88   `"Ybbd8"'  `"YbbdP"'  
                 88                                                                   
                 88                                                                                                                                                                                                       
"""""""";

 public void PrintLogo()
 {
  if (AnsiConsole.Console.Profile.Capabilities.Interactive && AnsiConsole.Console.Profile.Width < 90)
  {
   AnsiConsole.Console.Profile.Width = 90;
  }

  if (AnsiConsole.Console.Profile.Width >= 90)
  {
   AnsiConsole.Console.WriteLine(LargeAsciiLogo, new Style(Color.Turquoise2, null, Decoration.Bold));
  }
 }
}