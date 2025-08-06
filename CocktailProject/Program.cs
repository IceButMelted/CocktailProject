using var game = new CocktailProject.Game1();
using var game2 = new CocktailProject.TestImportArt();

int runGame = 1;
if (runGame == 1)
{
    game.Run();
}
if( runGame == 2)
{
    game2.Run();
}
