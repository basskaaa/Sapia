param([String]$workingDirectory = ".",
    [String]$buildVersion = "XYZ")

$ErrorActionPreference = "Stop"

$Directory = $workingDirectory + "/Sapia.Unity/Assets/Scripts/Config/"

New-Item -ItemType Directory -Force -Path $Directory > $null

$Content = 
@"
namespace Assets.Scripts.Config
{
    public class BuildVersion
    {
        public const string Version = "VERSION_NUMBER";
    }
}
"@;

$Content = $Content -replace "VERSION_NUMBER", $buildVersion

Set-Content ($Directory + "BuildVersion.cs") $Content