#!/bin/bash
sudo systemctl stop SilbernetzBackend
cd /home/fapo/Silbernetz
/usr/bin/git pull
cd /home/fapo/Silbernetz/Silbernetz
dotnet clean
cp -p /home/fapo/appsettings.json /home/fapo/Silbernetz/Silbernetz/appsettings.json
dotnet publish --configuration Release
sudo systemctl start SilbernetzBackend
cd /home/fapo/Silbernetz/ClientApp/
/usr/bin/npm install
/usr/local/bin/ng build --prod
sudo systemctl status SilbernetzBackend
