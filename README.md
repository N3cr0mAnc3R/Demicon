1. Open folder RansomUserServer.
2. Open Cmd.
3. insert "dotnet run".
4. Open http://localhost:5046/
5. Api available http://localhost:5046/api/user

For deployment can follow https://docs.microsoft.com/aspnet/core/host-and-deploy/?view=aspnetcore-6.0. 
Preferences for max number of users in country should be set by the developer. There is a possibility to put configuration file for scaling maximum users and/or countries. To prevent exceed memory it is must be set. 
If number of users is set, there must be set behavior for reset users (for example, first user should be replaced by the new one).
