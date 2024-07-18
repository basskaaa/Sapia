using Sapia.Game.Hack.Configuration;
using Sapia.Game.Hack.Status;
using Sapia.Game.TestConsole;

var theRockConfiguration = new CharacterConfiguration
{
    Name = "Dwayne 'The Rock' Johnson",
    LevelConfigurations = new()
    {
        {1, new()
        {
            ClassId = "Fighter"
        }},
        {2, new()
        {
            ClassId = "Fighter"
        }}
    }
};


var typeData = TypeDataFactory.CreateTypeData();
var characterStatusService = new CharacterService(typeData);

var theRockStatus = characterStatusService.CompileCharacter(theRockConfiguration, ["Parry", "Whirlwind"]);