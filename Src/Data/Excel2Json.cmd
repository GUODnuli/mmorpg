chcp 65001 > nul
json-excel\json-excel json Tables\ Data\

if not exist "..\Client\Data\" (
    echo Creating directory ..\Client\Data\
    mkdir "..\Client\Data\"
)

if not exist "..\Server\GameServer\GameServer\bin\Debug\Data\" (
    echo Creating directory ..\Server\GameServer\GameServer\bin\Debug\Data\
    mkdir "..\Server\GameServer\GameServer\bin\Debug\Data\"
)

@copy Data\CharacterDefine.txt ..\Client\Data\
@copy Data\MapDefine.txt ..\Client\Data\
@copy Data\LevelUpDefine.txt ..\Client\Data\
@copy Data\SpawnRuleDefine.txt ..\Client\Data\
@copy Data\NPCDefine.txt ..\Client\Data\
@copy Data\ItemDefine.txt ..\Client\Data\

@copy Data\CharacterDefine.txt ..\Server\GameServer\GameServer\bin\Debug\Data\
@copy Data\MapDefine.txt ..\Server\GameServer\GameServer\bin\Debug\Data\
@copy Data\LevelUpDefine.txt ..\Server\GameServer\GameServer\bin\Debug\Data\
@copy Data\SpawnRuleDefine.txt ..\Server\GameServer\GameServer\bin\Debug\Data\
@copy Data\NPCDefine.txt ..\Server\GameServer\GameServer\bin\Debug\Data\
@copy Data\ItemDefine.txt ..\Server\GameServer\GameServer\bin\Debug\Data\

pause