set PATH=c:\python27;%PATH%

rd /s /q "mavlink"

set PYTHONPATH=..\..\ExtLibs\Mavlink
python -m pymavlink.tools.mavgen --lang=CS --wire-protocol=2.0 --output=.\mavlinkRaw message_definitions\InertialLabs.xml

pause