using Sapia.Game.Hack.Configuration;
using Sapia.Game.TestConsole;

var typeData = TypeDataFactory.CreateTypeData();

var theRock = new CharacterConfiguration
{
    Name = "Dwayne 'The Rock' Johnson",
    LevelConfigurations = new()
    {
        {1, new LevelConfiguration
        {
            ClassId = "Fighter"
        }},
        {2, new LevelConfiguration
        {
            ClassId = "Fighter"
        }}
    }
};

