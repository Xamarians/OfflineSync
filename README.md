# Overview
Repository contains two Xamarin.Forms sample applications that store data locally when device is offline and sync to server when connected to internet. 
1. AzureOfflineSyncDemo : Uses azure mobile sdk and connet internally to fetch and store data. Backend service is published in azure 
2. OfflineSyncDemo : Uses own business logic to save data offline and sync data to server when connected to online. 
3. SyncPocService: .Net Core web api used to store data in sql database.

## Azure Offline Sync
*Download Android App 
https://drive.google.com/open?id=1n_kqWbeFc0BQNiauI2SVlgpuBuDMnaGy

Server data can be check from here: http://syncpoc.aspcore.net/Home/Employees


## Custom Offline Sync
*Download Android App
https://drive.google.com/open?id=19RvT90T6edf1z-zf-afzIVWNm_09sodP

Server data can be check from here: http://syncpoc.aspcore.net/Home/Users

*Technology Stack*
Web Api: Asp.Net Core 2.1
Mobile App: Xamarin.Forms (Android and iOS)
Database: SQLite and MSSql Server
Server: Azure Cloud
