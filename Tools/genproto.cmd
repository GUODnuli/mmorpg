"protoc-3.2.0-win32/bin/protogen" --proto_path=../Src/Lib/proto --csharp_out=../Src/Lib/Protocol message.proto

xcopy /Y /E /I "..\Src\Lib\Protocol\bin\Debug\*" "..\Src\Client\Assets\References"