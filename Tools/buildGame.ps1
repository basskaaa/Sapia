Set-Location Sapia.Game

dotnet build -c Release
dotnet test

$src = "./Sapia.Game\bin\Release\netstandard2.1\Sapia.Logic.dll"
$dest = "../Sapia/Assets"

Copy-Item  $src $dest
Set-Location ..