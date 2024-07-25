Set-Location Sapia.Game

dotnet build -c Release
dotnet test

$src = "./Sapia.Game\bin\Release\netstandard2.1\Sapia.Game.dll"
$dest = "../Sapia.Unity/Assets"

Copy-Item  $src $dest
Set-Location ..