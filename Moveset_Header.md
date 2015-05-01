# Introduction #

Each moveset .dat file has a section marked "data". This section contains offsets to the every section in the moveset. This section is a different length per character, and files edited with PSA sometimes give OpenSA3 some trouble here. Heres a list of data header lengths for the default files.

**Dantarion says**:
> As a funny note, the reason PSA cannot open Dedede's file is because the "data" section is not first on the list of sections in his file.

## Data Section Lengths ##
| .pac Name | "data" header length | # of entries (length/4) | extra entries |
|:----------|:---------------------|:------------------------|:--------------|
| FitCaptain | 156 | 39 | 7 |
| FitDedede | 188 | 47 | 15 |
| FitDiddy | 176 | 44 | 12 |
| FitDonkey | 160 | 40 | 8 |
| FitFalco | 168 | 42 | 10 |
| FitFox | 176 | 44 | 12 |
| FitGameWatch | 188 | 47 | 15 |
| FitGanon | 148 | 37 | 5 |
| FitGKoopa | 148 | 37 | 5 |
| FitIke | 156 | 39 | 7 |
| FitKirby | 396 | 99 | 67 |
| FitKoopa | 144 | 36 | 4 |
| FitLink | 204 | 51 | 19 |
| FitLucario | 168 | 42 | 10 |
| FitLucas | 176 | 44 | 12 |
| FitLuigi | 156 | 39 | 7 |
| FitMario | 164 | 41 | 9 |
| FitMarth | 148 | 37 | 5 |
| FitMetaknight | 144 | 36 | 4 |
| FitNess | 180 | 45 | 13 |
| FitPeach | 180 | 45 | 13 |
| FitPikachu | 168 | 42 | 10 |
| FitPikmin | 176 | 44 | 12 |
| FitPit | 176 | 44 | 12 |
| FitPokeFushigisou | 152 | 38 | 6 |
| FitPokeLizardon | 156 | 39 | 7 |
| FitPokeTrainer | 156 | 39 | 7 |
| FitPokeZenigame | 156 | 39 | 7 |
| FitPopo | 196 | 49 | 17 |
| FitPurin | 136 | 34 | 2 |
| FitRobot | 180 | 45 | 13 |
| FitSamus | 212 | 53 | 21 |
| FitSheik | 160 | 40 | 8 |
| FitSnake | 204 | 51 | 19 |
| FitSonic | 156 | 39 | 7 |
| FitSZerosuit | 172 | 43 | 11 |
| FitToonLink | 204 | 51 | 19 |
| FitWario | 164 | 41 | 9 |
| FitWarioMan | 160 | 40 | 8 |
| FitWolf | 176 | 44 | 12 |
| FitYoshi | 176 | 44 | 12 |
| FitZakoBall | 128 | 32 | 0 |
| FitZakoBoy | 128 | 32 | 0 |
| FitZakoChild | 128 | 32 | 0 |
| FitZakoGirl | 128 | 32 | 0 |
| FitZelda | 164 | 41 | 9 |

## What does this mean? ##
Lets take a sec and look at this. The smallest data sections belong to the Zako's, so they probably represent the minimal data needed for a character. The only character who comes close is Purin(Jigglypuff) who has two extra entries.

On the other side of the equation, Kirby has waaay more than anyone else, and Samus, Link, Zelda, etc, all have higher values.

Why is this? These extra values have to do with articles, which can span from things like projectiles to other random things that some characters have and some don't. Anyways, for now lets focus on the 32 values all characters share.